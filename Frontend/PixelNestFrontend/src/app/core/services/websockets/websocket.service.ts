import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Message } from '../../dto/message.dto';
import { ChatStateService } from '../states/chat-state.service';
import { DashboardStateService } from '../states/dashboard-state.service';
import { UserSessionService } from '../user-session/user-session.service';
import { LastSendersDto } from '../../dto/lastSenders.dto';
import { NotificationStateService } from '../states/notification-state.service';
import { NotificationDto } from '../../dto/notification.dto';
import { WebSocketMessage } from '../../dto/websocketMessage.dto';
import { InboxStateService } from '../states/inbox-state.service';


@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  private _socket:WebSocket | null = null;
  messageData: Message = {
    sender:'',
    receiver:'',
    message:'',
    roomID:'',
    dateSent:new Date(),
    source:'',
    isSeen:true,
    messageID: 0,
    userID: ''
    
  };
  messages:Message[] = []
  lastSenderIDs:LastSendersDto[] =[]
  lastSenderIDObj:LastSendersDto = {currentUser:'', senders:[]} 
  newNotification:NotificationDto = {
    username:'',
    message:'',
    date:new Date(),
    notificationID:0,
    postID:0,
    imagePath:[]
  }
  constructor(
    private _chatState:ChatStateService,
    private _dashboardState:DashboardStateService,
    private _userSession:UserSessionService,
    private _notificationState:NotificationStateService,
    private _inboxState:InboxStateService
  ) { }

  connect():void{
    
    const wsURL = `${environment.wsUrl}`;
    this._socket = new WebSocket(wsURL);

    this._socket.onopen = () => {
    }
    this._socket.onmessage = (event) => {
      var data = JSON.parse(event.data);
      this._processWebsocketMessage(data);
    };

    this._socket.onclose = (event) => {
    };

    this._socket.onerror = (error) => {
    };
   
  }
  private _processWebsocketMessage(data: any) {
    this._formMessageData(data);
    switch (data.Type) {
      case "Typing":
        this._chatState.setIsTyping(true);
        return;

      case "StopTyping":
        this._chatState.setIsTyping(false);
        return;

      case "Leave":
        if(data.ChatID == this._chatState.getCurrentChatID()){
          this._chatState.setSeenStatus(false);
        }
        
        return;
      
      case "Enter":
        if(data.ChatID == this._chatState.getCurrentChatID()){
          this._chatState.setSeenStatus(true);
        }
        return;

      case "Direct":
        this._proccessDirectMessage();
        break;

      case "Status":
        this._chatState.updateActiveUsers(data);
        return;

      case "ActiveUsers":
        this._chatState.setActiveUsers(data.Users);
        return;

      case "Like":
      case "Comment":
      case "Follow":
        this._processNewNotification(data);
        return;
    }
    this._chatState.setMessages(this.messageData)
  }

  close(): void {
    if (this._socket) {
      this._socket.close();
    }
  }

  sendTypingNotification(receiverClientGuid:string, senderClientGuid:string, content:string, type:string, roomID:string){
    if(this._socket && this._socket.readyState == WebSocket.OPEN){
      const message:WebSocketMessage = {
        TargetUser:receiverClientGuid,
        SenderUser:senderClientGuid,
        Type:type,
        Content:content,
        SenderUsername:'',
        RoomID:roomID
      }
      this._socket.send(JSON.stringify(message));
    }
  }

  private _formMessageData(data:any){
    this.messageData.message = data.Content
    this.messageData.sender = data.SenderUsername
    this.messageData.receiver = data.TargetUser
    this.messageData.roomID = data.ChatID
    this.messageData.dateSent = data.Date
    this.messageData.userID = data.SenderUser
  }

  private _processNewNotification(data:any){

    this.newNotification.message = `${data.SenderUsername} ${data.Content}`;
    this.newNotification.username = data.SenderUsername
    this.newNotification.date = data.Date
    this._notificationState.updateNotifications(this.newNotification);

    this._notificationState.setNewNotification(true)
    this._notificationState.setNotificationType(data.Type)
    this._notificationState.updateNotification(1);

  }

  private _proccessDirectMessage(){
      this._dashboardState.setMessage(this.messageData.message)
      this._dashboardState.setSender(this.messageData.sender)
      this._dashboardState.setChatID(`${this.messageData.userID}/${this.messageData.roomID}`)
      this._dashboardState.setIsNotification(true);
      this._inboxState.updateInbox(this.messageData)
      this._processLastIDS(this.messageData.userID)
  }

  private _processLastIDS(sender:string){
    this.lastSenderIDs = this._chatState.getLastIDS()
    
    let currentUser = this._userSession.getFromCookie("sessionID")
    this.lastSenderIDObj.currentUser = currentUser;
    this.lastSenderIDObj.senders.push(sender);

    const matched = this.lastSenderIDs.find(a => a.currentUser == this.lastSenderIDObj.currentUser)
    if(!matched?.senders.includes(sender) || matched == null) this._chatState.updateNewMessages(1);
    if(matched) matched?.senders.push(...this.lastSenderIDObj.senders)
    else{
      const copy = {...this.lastSenderIDObj}
      this.lastSenderIDs.push(copy);
      
    }
    this._userSession.setToCookie("ids", JSON.stringify(this.lastSenderIDs));
    this.lastSenderIDObj.senders = []
    
  }
}
