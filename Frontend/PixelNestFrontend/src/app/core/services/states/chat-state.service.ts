import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Users } from '../../dto/users.dto';
import { ProfileUser } from '../../dto/profileUser.dto';
import { Message } from '../../dto/message.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatStateService {
  private _newMessages = new BehaviorSubject<number>(0)
  updateNewMessages$ = this._newMessages.asObservable();
  private _isUserInRoom = new BehaviorSubject<boolean>(false)
  isUserInRoom$ = this._isUserInRoom.asObservable();

  private _chatStateUser = new BehaviorSubject<ProfileUser>(
    {
      username:'',
      totalPosts:0,
      followers:0,
      followings:0,
      name:'',
      lastname:''
    }
  );
  
  chatStateUser = this._chatStateUser.asObservable();


  private _chatStateMessage = new BehaviorSubject<Message>({
    sender:'',
    receiver:'',
    message:'',
    roomID:'',
    dateSent: new Date(),
    source:'',
    isSeen: true,
    messageID: 0
  });
  chatStateMessage = this._chatStateMessage.asObservable()
  setUser(value:ProfileUser){
    this._chatStateUser.next(value);
  }

  setMessages(value:Message){
    let messages = this._chatStateMessage.getValue()
    messages = value;
    this._chatStateMessage.next(messages);
  }

  setIsUserInRoom(value:boolean){
    this._isUserInRoom.next(value);
  }
  resetNewMessages(){
    this._newMessages.next(0)
  }
  updateNewMessages(value:number){
    let number = this._newMessages.getValue()
    number += value;

    this._newMessages.next(number);
  }
  constructor() { }
}
