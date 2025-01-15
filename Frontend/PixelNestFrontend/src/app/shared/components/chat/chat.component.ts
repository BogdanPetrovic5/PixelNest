import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject, takeUntil, tap } from 'rxjs';
import { Message } from 'src/app/core/dto/message.dto';
import { ProfileUser } from 'src/app/core/dto/profileUser.dto';
import { ChatService } from 'src/app/core/services/chat/chat.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy{
  @ViewChild('chatContainer') private chatContainer!: ElementRef;
  chatRoomID:string = ""
  reverseChatRoomID:string = ""
  private destroy$ = new Subject<void>();
  username:string = ""
  message:string = ""
  messages:Message[] = []
  user:ProfileUser = {
    username: '',
    followers: 0,
    followings: 0,
    totalPosts:0,
    name: '',
    lastname:''

  }

   messageData: Message = {
      sender:'',
      receiver:'',
      message:'',
      roomID:'',
      dateSent: new Date(),
      source: ''
    };
  constructor(
    private _chatState:ChatStateService,
    private _route:ActivatedRoute,
    private _userService:UserService,
    private _chatService:ChatService,
    private _userSession:UserSessionService
  ){}
  ngOnDestroy(): void {
      this._chatService.leaveRoom(this.username).subscribe({
        next:response=>{
          console.log(response)
        }
      })
  }
  ngOnInit(): void {
    this._initilizeComponent()

  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    const container = this.chatContainer.nativeElement;
    container.scrollTop = container.scrollHeight;
  }
  sendMessage(){
    this.messageData.receiver = this.username;
    this.messageData.sender = this._userSession.getFromCookie("username")
    this.messageData.message = this.message;
   
    
    this._chatService.sendMessage(this.messageData).subscribe({
      next:response=>{
        const messageCopy = { ...this.messageData };
        this.messages.push(messageCopy)
        this.messageData.message = "";
        this.message = ""
      }
    })
  
  }
  private _initilizeComponent(){
   
    this._route.paramMap
        .pipe(
          takeUntil(this.destroy$), 
          tap(params => {
            this.username = params.get("username") ?? ""
          
            this._loadData();
            
          })
        ).subscribe()
    this.chatRoomID = `${this._userSession.getFromCookie("username")}-${this.username}`
    this.reverseChatRoomID = `${this.username}-${this._userSession.getFromCookie("username")}`    
  }

  private _loadData(){
    this._userService.getUserData(this.username).subscribe({
      next:response=>{
        this.user = response;
        this._joinRoom();
      }
    })
  }
  private _loadMessages(){
    this._chatService.getMessages(this.username).subscribe({
      next:response=>{
        this.messages = response;
        console.log(this.messages);
      }
    })
  }
  private _subsribeToWebSocket(){
      this._chatState.chatStateMessage.subscribe({
        next:response=>{
          if(response.roomID == this.chatRoomID || response.roomID == this.reverseChatRoomID){
            const messageCopy = { ...response };
            this.messages.push(messageCopy)
          }
        }
      })
  }
  private _joinRoom(){
    console.log(this.user.username)
    this._chatService.joinRoom(this.user.username).subscribe({
      next:response=>{
        console.log(response);
        this._loadMessages();
        this._subsribeToWebSocket();
      }
    })
  }
}
