import { DatePipe } from '@angular/common';
import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil, tap } from 'rxjs';
import { ActiveUsers } from 'src/app/core/dto/activeUsers.dto';
import { Message } from 'src/app/core/dto/message.dto';
import { MessageSeen } from 'src/app/core/dto/messageSeen.dto';
import { ProfileUser } from 'src/app/core/dto/profileUser.dto';
import { ChatService } from 'src/app/core/services/chat/chat.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { ChatFacadeService } from 'src/app/core/services/states/facade/chat-facade.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';
import { WebsocketService } from 'src/app/core/services/websockets/websocket.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  providers:[DatePipe],
})
export class ChatComponent implements OnInit, OnDestroy{
  @ViewChild('chatContainer') private chatContainer!: ElementRef;
  activeUsers:ActiveUsers[] = []
  chatRoomID:string = ""
  reverseChatRoomID:string = ""
  private destroy$ = new Subject<void>();
  clientID:string = ""
  chatID:string = ""
  message:string = ""
  public showOptionIndex: number | null = null
  messages:Message[] = []
  user:ProfileUser = {
    username: '',
    followers: 0,
    followings: 0,
    totalPosts:0,
    name: '',
    lastname:'',
    clientGuid:'',
    canFollow:false,
    canEdit:false,
    chatID:'',
    email:'',
    profileImagePath:''
  }
  messageSeen:MessageSeen = {
    messageID:[]
  }
  messageData!: Message;
  currentClientGuid:string = ""
  isTyping:boolean = false;
  constructor(
    private _route:ActivatedRoute,
    private _userSession:UserSessionService,
    private _datePipe:DatePipe,
    private _chatFacadeService:ChatFacadeService,
    private _router:Router,
  ){}
  ngOnDestroy(): void {
    this._chatFacadeService.leaveRoom(this?.user?.clientGuid)
    this._chatFacadeService.cleanUp();
    this.destroy$.next;
    this.destroy$.complete;
  }
  ngOnInit(): void {
    this.currentClientGuid = this._userSession.getFromCookie('sessionID');
    this._route.paramMap
    .pipe(
      takeUntil(this.destroy$),
      tap(params => {
        this.clientID = params.get('clientID') ?? '';
        this.chatID = params.get('chatID') ?? '';
        console.log(this.chatID);
        this._chatFacadeService.initChat(this.clientID, this.chatID);

      })
    ).subscribe();
    this._chatFacadeService.messages$.pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next:response=>{
        this.messages = response;
        this._scrollToBottom();
      }
    })
    this._chatFacadeService.isTyping$.pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next:response=>{
        this.isTyping = response
        if(this.isTyping) this._scrollToBottom()
      }
    })
    this._chatFacadeService.activeUsers$.pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next:response=>{
        this.activeUsers = response
      }
    })
    this._chatFacadeService.user$.pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next:response=>{
        this.user = response;
    
      }
    })
  }
  sendMessage() {
    if (!this.message.trim()) return;

    const messageData = this._createDefaultMessage(); 
    messageData.userID = this.currentClientGuid;
    this._chatFacadeService.sendMessage(messageData, this.message).pipe(
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.message = "";
      this._scrollToBottom();
    });
  }
  private _scrollToBottom(): void {
    setTimeout(() => {
      const container = this.chatContainer.nativeElement;
      container.scrollTop = container.scrollHeight;
    }, 0);
  }
  toggleOptions(index:number){
      if(this.showOptionIndex == null) this.showOptionIndex = index 
      else this.showOptionIndex = null;
      
  }
  hideOptions(){
    setTimeout(() => {
      this.showOptionIndex = null;
    }, 200); 
  }
  typing(){
    this._chatFacadeService.sendTypingNotification()
  }
  stopTyping(){
     this._chatFacadeService.stopTypingNotification()
  }
  navigate(url:string){
    this._router.navigate([`${url}`])
  }

  
  trackByFn(index: number, item: any): number {
    return item.messageID || index
  } 
  isActive(): boolean{
    const obj = this.activeUsers.find((a:any) => a.userID === this.clientID);
    if(obj != null && obj != undefined){
      return obj.isActive 
    } return false
  }

  formatDate(date:Date | string){
    
    let formattedDate = ""
    if(typeof(date) === "string"){
      date = date.replace(/Z$/, "");
    }
    if(date){
      const dateCopy = date;
    
      const dateObject = new Date(dateCopy)
      
      const utcDate = new Date(dateObject.getTime() - dateObject.getTimezoneOffset() * 60000);

    
      if (isNaN(utcDate.getTime())) {
        formattedDate = 'Invalid Date';
      } else {
        formattedDate = this._datePipe.transform(utcDate, 'd MMM \'at\' HH:mm') || 'Invalid Format';
      }
    }
    return formattedDate
  }


  private _createDefaultMessage(): Message {
    const currentDate = new Date();
   
    return {
      sender: '',
      receiver: '',
      message: '',
      roomID: '',
      dateSent: new Date(currentDate.getTime() - 3600000),
      source: '',
      isSeen: true,
      messageID: 0,
      userID:'',
      
    };
  }
}
