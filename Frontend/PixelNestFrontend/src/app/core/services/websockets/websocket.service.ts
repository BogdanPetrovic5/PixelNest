import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Message } from '../../dto/message.dto';
import { ChatStateService } from '../states/chat-state.service';
import { DashboardStateService } from '../states/dashboard-state.service';
import { UserSessionService } from '../user-session/user-session.service';
import { LastSendersDto } from '../../dto/lastSenders.dto';
import { NotificationStateService } from '../states/notification-state.service';


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
  constructor(
    private _chatState:ChatStateService,
    private _dashboardState:DashboardStateService,
    private _userSession:UserSessionService,
    private _notificationState:NotificationStateService
  ) { }

  connect():void{
    
    const wsURL = `${environment.wsUrl}`;
    this._socket = new WebSocket(wsURL);

    this._socket.onopen = () => {
      console.log("Websocket connection established!")
    }
    this._socket.onmessage = (event) => {
   
      var data = JSON.parse(event.data);
      
      this.messageData.message = data.Content
      this.messageData.sender = data.SenderUsername
      this.messageData.receiver = data.TargetUser
      this.messageData.roomID = data.RoomID
      this.messageData.dateSent = new Date()
      this.messageData.userID = data.SenderUser
     
      if(data.Type === "Direct"){
        this._proccessDirectMessage()
      }
      if(data.Type === "Status"){
        this._chatState.updateActiveUsers(data);
      }
      if(data.Type === "ActiveUsers"){
     
        this._chatState.setActiveUsers(data.Users);
      }
      
      if(data.Type === "Like" || data.Type === "Comment" || data.Type === "Follow"){
          this._notificationState.setNewNotification(true)
          this._notificationState.setNotificationType(data.Type)
          this._notificationState.updateNotification(1);
      }
      this._chatState.setMessages(this.messageData)

    };

    this._socket.onclose = (event) => {
      console.log('WebSocket connection closed:', event);
    };

    this._socket.onerror = (error) => {
      console.error('WebSocket error:', error);
    };
   
  }

  close(): void {
    if (this._socket) {
      this._socket.close();
    }
  }
  

  private _proccessDirectMessage(){
    this._dashboardState.setMessage(this.messageData.message)
    this._dashboardState.setSender(this.messageData.sender)
    this._dashboardState.setIsNotification(true);
    this._processLastIDS(this.messageData.userID)
    

  }

  private _processLastIDS(sender:string){
    this.lastSenderIDs = this._chatState.getLastIDS()
    
    let currentUser = this._userSession.getFromCookie("userID")
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
    
   
    console.log(this.lastSenderIDs)
    this.lastSenderIDObj.senders = []
    
  }
}
