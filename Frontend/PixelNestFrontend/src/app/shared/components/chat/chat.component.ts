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
    private _chatState:ChatStateService,
    private _route:ActivatedRoute,
    private _userService:UserService,
    private _chatService:ChatService,
    private _userSession:UserSessionService,
    private _datePipe:DatePipe,
    private _router:Router,
    private _websocketService:WebsocketService
  ){}
  ngOnDestroy(): void {
      this._chatService.leaveRoom(this.user.clientGuid).subscribe({
        next:response=>{
         
        }
      })
  }
  ngOnInit(): void {
    this.messageData = this._createDefaultMessage()
    this.currentClientGuid = this._userSession.getFromCookie("sessionID")
    
    this._initializeComponent()

  }

  ngAfterViewChecked(): void {
   
  }
  typing(){
    this._websocketService.sendTypingNotification(this.user.clientGuid, this.currentClientGuid, "is typing", "Typing", this.chatRoomID)
  }
  stopTyping(){
     this._websocketService.sendTypingNotification(this.user.clientGuid, this.currentClientGuid, "", "StopTyping", this.chatRoomID)
  }
  navigate(url:string){
    this._router.navigate([`${url}`])
  }
  private scrollToBottom(): void {
    setTimeout(() => {
      const container = this.chatContainer.nativeElement;
      container.scrollTop = container.scrollHeight;
    }, 0);
  }
  sendMessage(){
    this.messageData.receiver = this?.user?.clientGuid;
    this.messageData.message = this.message;
   
    if(this.messageData.message.length > 0){
      this._chatService.sendMessage(this.messageData).subscribe({
        next:response=>{
          this.messageData.userID = this.currentClientGuid
          this.messageData.dateSent = response.date
          console.log(this.messageData)
          if(response.isUserInRoom == true){
            this.messageData.isSeen = true
            
            
          }else this.messageData.isSeen = false;
          const messageCopy = { ...this.messageData };
          
        
          this.messages.push(messageCopy)
          
          this.messageData.message = "";
          this.message = ""
          this.scrollToBottom();
        }
      })
    }
    
  
  }
  private _initializeComponent(){
   
    this._route.paramMap
        .pipe(
          takeUntil(this.destroy$), 
          tap(params => {
            this.clientID = params.get("clientID") ?? ""
            this.chatID = params.get("chatID") ?? ""
            this._loadData();
            
          })
        ).subscribe()
    this._chatState.isTyping$.subscribe({
      next:response=>{
        this.isTyping = response;
        if(this.isTyping) this.scrollToBottom()
      }
    })

  }

  private _loadData(){
    this._userService.getUserData(this.clientID).subscribe({
      next:response=>{
        this.user = response;
        this._joinRoom();
        this._updateLastMessage()
      }
    })
    this._chatState.activeUsers$.subscribe({
      next:response=>{
        this.activeUsers = response;
      }
    })

  }
  private _updateLastMessage(){
    this._chatState.seenStatus$.subscribe({
      next:response=>{
        
        if(response){
          this.messages[this.messages.length - 1].isSeen = true;
        }
      }
    })
  }
  trackByFn(index: number, item: any): number {
    return item.messageID
  } 
  isActive(): boolean{
    const obj = this.activeUsers.find((a:any) => a.userID === this.clientID);
    if(obj != null && obj != undefined){
      return obj.isActive 
    } return false
  }
  private _loadMessages(){
    this._chatService.getMessages(this.chatID).subscribe({
      next:response=>{
        this.messages = response;
        
        this._loadSeenMessages();
        this.scrollToBottom();
      }
    })
  }
  private _subsribeToWebSocket(){
      this._chatState.chatStateMessage.subscribe({
        next:response=>{
          
          if(response.roomID == this.chatRoomID || response.roomID == this.reverseChatRoomID){
            const messageCopy = { ...response };
            
            this.messages.push(messageCopy)
            this.scrollToBottom()
          }
        }
      })
     
  }
  private _joinRoom(){
   
    this._chatService.joinRoom(this.user.clientGuid).subscribe({
      next:response=>{
        this.chatRoomID = response.roomID;
        this.reverseChatRoomID = response.reverserdRoomID
        this._loadMessages();
        this._subsribeToWebSocket();
        
      }
    })
  }
  private _loadSeenMessages(){
    for(let i = this.messages.length - 1; i >= 0;i--){
      this.messageSeen.messageID.push(this.messages[i].messageID)
    }
  
    this._chatService.markAsRead(this.messageSeen).subscribe({
      next:response=>{
        
        this._chatState.updateNewMessages(-1)
        this._proccessLastSenders()
      }
    });
  }
  private _proccessLastSenders(){
    let senders = this._userSession.getFromCookie("ids");
    if(senders){
      let sendersParsed = JSON.parse(senders);
      let currentUsername = this._userSession.getFromCookie("sessionID")
      const matched = sendersParsed.find((a:any) => a.currentUser === currentUsername)
   
      if (matched) {
        matched.senders = matched.senders.filter((sender:any) => sender !== this.clientID);
      }
    
      this._chatState.setLastIDS(sendersParsed);
      this._userSession.setToCookie("ids", JSON.stringify(sendersParsed));

    }
    

    
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
