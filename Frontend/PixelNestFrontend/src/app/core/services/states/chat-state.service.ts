import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Users } from '../../dto/users.dto';
import { ProfileUser } from '../../dto/profileUser.dto';
import { Message } from '../../dto/message.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatStateService {
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
    source:''
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
  constructor() { }
}
