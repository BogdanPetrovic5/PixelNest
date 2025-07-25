import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Users } from '../../dto/users.dto';
import { ProfileUser } from '../../dto/profileUser.dto';
import { Message } from '../../dto/message.dto';
import { LastSendersDto } from '../../dto/lastSenders.dto';
import { ActiveUsers } from '../../dto/activeUsers.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatStateService {
  private _newMessages = new BehaviorSubject<number>(0)
  updateNewMessages$ = this._newMessages.asObservable();
  private _isUserInRoom = new BehaviorSubject<boolean>(false)
  isUserInRoom$ = this._isUserInRoom.asObservable();

  private _lastSenderIDS = new BehaviorSubject<LastSendersDto[]>([])
  lastSenderIDS = this._lastSenderIDS.asObservable()

  private _isTyping = new BehaviorSubject<boolean>(false);
  isTyping$ = this._isTyping.asObservable()

  private _unsentMessage = new BehaviorSubject<number | null>(null);
  unsentMessage$ = this._unsentMessage.asObservable();
  private _chatStateUser = new BehaviorSubject<ProfileUser>(
    {
      username:'',
      totalPosts:0,
      followers:0,
      followings:0,
      name:'',
      lastname:'',
      clientGuid:'',
      canFollow:false,
      canEdit:false,
      chatID:'',
       email:'',
        profileImagePath:''
    }
  );

  private _activeUsers = new BehaviorSubject<ActiveUsers[]>([])
  activeUsers$ = this._activeUsers.asObservable();
  
  chatStateUser = this._chatStateUser.asObservable();

  private _currentChatID = new BehaviorSubject<string>("");
  
  private _chatStateMessage = new BehaviorSubject<Message>({
    sender:'',
    receiver:'',
    message:'',
    roomID:'',
    dateSent: new Date(),
    source:'',
    isSeen: true,
    messageID: 0,
    userID:'',
    canUnsend:false
  });
  chatStateMessage = this._chatStateMessage.asObservable()
  private _seenStatus = new BehaviorSubject<boolean>(false);
  seenStatus$ = this._seenStatus.asObservable();

  setUnsentMessage(messageID:number | null){
    this._unsentMessage.next(messageID);
  }
  getCurrentChatID(){
    return this._currentChatID.getValue()
  }
  setCurrentChatID(chatID:string){
    this._currentChatID.next(chatID);
  }
  setSeenStatus(value:boolean){
    this._seenStatus.next(value)
  }

  setIsTyping(value:boolean){
    this._isTyping.next(value)
  }
  
  updateActiveUsers(value:any){

    let users = this._activeUsers.getValue()

    let user = users.find((a:any) => a.userID === value.UserID);
    if(user){
      user.isActive = value.IsActive;
    }else{
      users.push({
        userID:value.UserID,
        isActive:value.IsActive
      });
    }
    
    this._activeUsers.next(users);

  }

  setActiveUsers(value:ActiveUsers[]){
    this._activeUsers.next(value)
  }
  setUser(value:ProfileUser){
    this._chatStateUser.next(value);
  }

  updateLastIDS(value:LastSendersDto){
    let ids = this._lastSenderIDS.getValue();
    ids.push(value);

    this._lastSenderIDS.next(ids);
  }
  setLastIDS(value:LastSendersDto[]){
    this._lastSenderIDS.next(value)
  }

  getLastIDS() : LastSendersDto[]{
    return this._lastSenderIDS.getValue();
  }
  resetLastIDS(){
    this._lastSenderIDS.next([]);
  }
  
  setMessages(value:Message){
    console.log("Received: ", value);
    let message = this._chatStateMessage.getValue()
    message = value;
    this._chatStateMessage.next(message);
  }
  setIsUserInRoom(value:boolean){
    this._isUserInRoom.next(value);
  }
  resetNewMessages(){
    this._newMessages.next(0)
  }
  setNewMessages(value:number){
    this._newMessages.next(value)
  }
  updateNewMessages(value:number){
    let number = this._newMessages.getValue()
    
    if((number + value) >= 0) number += value;

    this._newMessages.next(number);
  }
  constructor() { }
}
