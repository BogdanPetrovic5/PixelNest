import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Chats } from '../../dto/chats.dto';
import { ChatService } from '../chat/chat.service';
import { Message } from '../../dto/message.dto';

@Injectable({
  providedIn: 'root'
})
export class InboxStateService {
  private message:Message = {
    message: '',
    receiver: '',
    sender: '',
    roomID: '',
    dateSent: new Date(),
    source: '',
    userID: '',
    messageID: 0,
    isSeen: false,
    canUnsend:false
  }

  private _inboxState = new BehaviorSubject<Chats[]>([])
  inboxState$ = this._inboxState.asObservable();

  constructor(private _inboxService:ChatService) { }
  updateInbox(massageData:any){
    this._formTheMessage(massageData);
    
    let inbox = this._inboxState.getValue();
    console.log(this.message)
    let room = inbox.find(chat => chat.chatID == massageData.roomID);
    console.log(room)
    if(room == null || room == undefined){
      let newChat:Chats = {
        chatID: this.message.roomID,
        userID: this.message.userID,
        username:this.message.sender,
        messages: [this.message]
      }
      inbox = [newChat, ...inbox];
    }else{
      room.messages[0] = this.message;
    }
    this._inboxState.next(inbox)
  }

  loadInbox(){
    this._inboxService.getChats().subscribe({
      next:response=>{
        this._inboxState.next(response);
      }
    })
  }

  private _formTheMessage(messageData:any){
    console.log("Mesasge from websocket", messageData.dateSent)
    this.message.dateSent = messageData.dateSent
    this.message.message = messageData.message
    this.message.isSeen = false
    this.message.roomID = messageData.roomID
    this.message.userID = messageData.userID
    this.message.receiver = messageData.receiver
    this.message.sender = messageData.sender
  }
}
