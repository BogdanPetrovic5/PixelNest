import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Message } from '../../dto/message.dto';
import { ChatStateService } from '../states/chat-state.service';
import { DashboardStateService } from '../states/dashboard-state.service';


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
    messageID: 0
  };
  messages:Message[] = []
  lastSenderID:string[] = []
  constructor(
    private _chatState:ChatStateService,
    private _dashboardState:DashboardStateService,
   
  ) { }

  connect(userID:string):void{
    const wsURL = `${environment.wsUrl}?userID=${userID}`;
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
     
      if(data.Type === "Direct"){
        this._dashboardState.setMessage(this.messageData.message)
        this._dashboardState.setSender(this.messageData.sender)
        this._dashboardState.setIsNotification(true);
        if(!this.lastSenderID.includes(this.messageData.sender, 0)) this._chatState.updateNewMessages(1);
        this.lastSenderID.push(this.messageData.sender)
        
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
 
}
