import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { data } from '@maptiler/sdk';
import { ActiveUsers } from 'src/app/core/dto/activeUsers.dto';
import { Chats } from 'src/app/core/dto/chats.dto';
import { Message } from 'src/app/core/dto/message.dto';
import { ChatService } from 'src/app/core/services/chat/chat.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
@Component({
  selector: 'app-inbox',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.scss']
})
export class InboxComponent implements OnInit{
  currentUser:string = ""
  text:string =""
  receiver:string = ""



  chats:Chats[] = []
  activeUsers:ActiveUsers[] = []
  constructor(
    private _userSession:UserSessionService, 
    private _chatService:ChatService,
    private _router:Router,
    private _chatState:ChatStateService
  ){}
  ngOnInit():void{
    this.currentUser = this._userSession.getFromCookie("username")
    this._initializeComponent();
  }
  chatNavigation(clientParameter:string, chatParameter:string){
    this._router.navigate([`/Chat/${clientParameter}/${chatParameter}`])
  }
  // timeAgo(date: Date): string {
  //   const now = Date.now();

  //   const messageDate = new Date(date).getDate()
 
  //   const diffInSeconds = Math.floor((now - messageDate) / 1000);

  //   if (diffInSeconds < 60) {
  //     return `${diffInSeconds} seconds ago`;
  //   }
  
  //   const diffInMinutes = Math.floor(diffInSeconds / 60);
  
  //   if (diffInMinutes < 60) {
  //     return `${diffInMinutes} minutes ago`;
  //   }
  
  //   const diffInHours = Math.floor(diffInMinutes / 60);
  
  //   if (diffInHours < 24) {
  //     return `${diffInHours} hours ago`;
  //   }
  
  //   const diffInDays = Math.floor(diffInHours / 24);
  
  //   return `${diffInDays} days ago`;
  // }

  navigate(){
    this._router.navigate(["Dashboard"])
  }
  isActive(source:string): boolean{
    const obj = this.activeUsers.find((a:any) => a.userID === source);
    if(obj != null && obj != undefined){
      return obj.isActive 
    } return false
  }
  private _initializeComponent(){
    this._chatService.getChats().subscribe({
      next:response=>{
        console.log("Chat response: ", response)
        this.chats = response;
        console.log(this.chats)
      }
    })
    this._chatState.activeUsers$.subscribe({
      next:response=>{
        this.activeUsers = response
       console.log(this.activeUsers)
      }
    })
  }

}
