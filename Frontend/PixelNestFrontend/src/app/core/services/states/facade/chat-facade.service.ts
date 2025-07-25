import { Injectable } from '@angular/core';
import { ChatService } from '../../chat/chat.service';
import { UserSessionService } from '../../user-session/user-session.service';
import { UserService } from '../../user/user.service';
import { WebsocketService } from '../../websockets/websocket.service';
import { ChatStateService } from '../chat-state.service';
import { BehaviorSubject, filter, Subscription, switchMap, tap } from 'rxjs';
import { ProfileUser } from 'src/app/core/dto/profileUser.dto';
import { Message } from 'src/app/core/dto/message.dto';
import { MessageSeen } from 'src/app/core/dto/messageSeen.dto';

@Injectable({
  providedIn: 'root'
})

export class ChatFacadeService {
  public messages$ = new BehaviorSubject<Message[]>([]);
  private _subs = new Subscription();
  private _websocketUnsentMessageID = new BehaviorSubject<number | null>(null);
  public websocketUnsentMessageID$ = this._websocketUnsentMessageID.asObservable();
  public user$ = new BehaviorSubject<ProfileUser>({
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
  )
  public isTyping$ = this._chatState.isTyping$;
  public activeUsers$ = this._chatState.activeUsers$;

  private _chatRoomID = '';
  private _reverseChatRoomID = '';
  private currentClientGuid = '';

  
  constructor( 
    private _chatService: ChatService,
    private _userService: UserService,
    private _chatState: ChatStateService,
    private _websocket: WebsocketService,
    private _session: UserSessionService
  ) { 
    this.currentClientGuid = _session.getFromCookie('sessionID');
  }
  cleanUp(){
    this._subs.unsubscribe();
    this._chatState.setCurrentChatID("");
    this.messages$.next([]);
  }

  initChat(clientID:string, chatID:string){
    this._subs = new Subscription();
    this._chatRoomID = chatID;
    this._chatState.setCurrentChatID(this._chatRoomID)
    this._subs.add(
      this._userService.getUserData(clientID).pipe(
        tap(user => this.user$.next(user)),
        switchMap(user => this._chatService.joinRoom(user.clientGuid)),
        tap(room => {
        }),
        tap(() => {
          const seenSubscription = this._chatState.seenStatus$.subscribe(seen => {
            if (seen && this.messages$.value.length > 0) {
              const updated = [...this.messages$.value];
              updated[updated.length - 1].isSeen = true;
              this.messages$.next(updated);
            }
          });

          const msgSubscription = this._chatState.chatStateMessage.pipe(
            filter(message => 
              message.roomID === this._chatRoomID || message.roomID === this._reverseChatRoomID
            ),
            tap(message => {
              const updated = [...this.messages$.getValue(), { ...message }];
              this.messages$.next(updated);
            })
          ).subscribe();
          const unsentSubscription = this._chatState.unsentMessage$.pipe(
            tap((messageID)=>{
              if(messageID != null){
                this._websocketUnsentMessageID.next(messageID)
              }

              
            })
          ).subscribe();
          this._subs.add(seenSubscription);
          this._subs.add(msgSubscription);
          this._subs.add(unsentSubscription);
        }),
        switchMap(() => this._chatService.getMessages(chatID)),
        tap(messages =>{
          this.messages$.next(messages);
          this._markAsRead(messages);
          this._chatState.updateNewMessages(-1);
          
        }),
        tap(()=> this._processLastSenders(clientID))
      ).subscribe()
    )
  }
  sendTypingNotification() {
    this._websocket.sendTypingNotification(
      this.user$.value?.clientGuid || '',
      this.currentClientGuid,
      'is typing',
      "Typing",
      this._chatRoomID
    );
  }

  stopTypingNotification() {
    this._websocket.sendTypingNotification(
       this.user$.value?.clientGuid || '',
      this.currentClientGuid,
      '',
      "StopTyping",
      this._chatRoomID
    );
  }
  sendMessage(message:Message, messageText:string){
    message.message = messageText;
    message.dateSent = new Date(new Date().getTime() - 3600000); 
    message.receiver = this.user$.value?.clientGuid || '';
    message.roomID = this._chatRoomID;

    return this._chatService.sendMessage(message).pipe(
      tap(response => {
        message.isSeen = response.isUserInRoom;
        message.dateSent = response.date;
        message.messageID = response.messageID
        const newMessages = [...this.messages$.getValue(), { ...message }];
        this.messages$.next(newMessages);
      })
    );
  }

  deleteForMe(messageID:number){
    this._filterMessages(messageID);
    this._subs.add(
      this._chatService.deleteForMe(messageID).subscribe()
    )
  }
  unsend(messageID:number){
    this._filterMessages(messageID)
    this._subs.add(
      this._chatService.unsend(messageID).subscribe()
    )
    
  }

  handleWebSocketUnsentMessage(messageID:number){
    this._filterMessages(messageID);
    this._chatState.setUnsentMessage(null);
  }
  leaveRoom(clientGuid:string){
    this._subs.add(
      this._chatService.leaveRoom(clientGuid).subscribe()
    )
  }
  private _filterMessages(messageID:number){
    let messages = this.messages$.getValue();
    const filteredArray = messages.filter(m => m.messageID !== messageID);
    this.messages$.next(filteredArray);
  }
  private _processLastSenders(clientID: string) {
    let senders = this._session.getFromCookie('ids');
    if (senders) {
      let sendersParsed = JSON.parse(senders);
      let currentID = this._session.getFromCookie('sessionID');
      const matched = sendersParsed.find((x: any) => x.currentUser === currentID);
      if (matched) {
        matched.senders = matched.senders.filter((s: any) => s !== clientID);
      }
      this._chatState.setLastIDS(sendersParsed);
      this._session.setToCookie('ids', JSON.stringify(sendersParsed));
    }
  }

  private _markAsRead(messages:Message[]){
    const messagesSeen:MessageSeen = {
      messageID: messages.map(msg => msg.messageID)
    }
    this._subs.add(
      this._chatService.markAsRead(messagesSeen).subscribe()
    )
    
  }
  
}
