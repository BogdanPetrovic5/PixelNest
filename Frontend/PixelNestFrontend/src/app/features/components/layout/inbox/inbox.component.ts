import { Component, OnDestroy, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { data } from '@maptiler/sdk';
import { debounceTime, distinctUntilChanged, Subject, Subscription, switchMap } from 'rxjs';
import { ActiveUsers } from 'src/app/core/dto/activeUsers.dto';
import { Chats } from 'src/app/core/dto/chats.dto';
import { Message } from 'src/app/core/dto/message.dto';
import { ChatService } from 'src/app/core/services/chat/chat.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { InboxStateService } from 'src/app/core/services/states/inbox-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
@Component({
  selector: 'app-inbox',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.scss']
})
export class InboxComponent implements OnInit, OnDestroy{
  private _searchTerms = new Subject<string>();

  currentUser:string = ""
  text:string =""
  receiver:string = ""
  
  subscription:Subscription | null = new Subscription();


  chats:Chats[] = []
  activeUsers:ActiveUsers[] = []
  constructor(
    private _userSession:UserSessionService, 
    private _chatService:ChatService,
    private _router:Router,
    private _chatState:ChatStateService,
    private _inboxState:InboxStateService
  ){}
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  ngOnInit():void{
    this.currentUser = this._userSession.getFromCookie("username")
    this._initializeComponent();
    this.searchChats();
    
  }
  chatNavigation(clientParameter:string, chatParameter:string){
    this._router.navigate([`/chat/${clientParameter}/${chatParameter}`])
  }
 

  navigate(){
    this._router.navigate(["dashboard"])
  }
  isActive(source:string): boolean{
    const obj = this.activeUsers.find((a:any) => a.userID === source);
    if(obj != null && obj != undefined){
      return obj.isActive 
    } return false
  }

  searchChats(){
    this._searchTerms.pipe(
          debounceTime(300),
          distinctUntilChanged(),
          
          switchMap((term:string)=>
            this._chatService.findChats(term)
          )
        ).subscribe(
          (chats)=>{

            this.chats = chats;
          }
        )
  }
  onSearch(event: Event){
    const input = event.target as HTMLInputElement;
    
    if(input.value.length > 0){
      this._searchTerms.next(input.value)
    }
    
  }
  private _initializeComponent(){
    this._inboxState.loadInbox()
    this.subscription?.add(
      this._inboxState.inboxState$.subscribe({
        next:response=>{
          this.chats = response;
        }
      })
      
    )
    this.subscription?.add(
      this._chatState.activeUsers$.subscribe({
        next:response=>{
          this.activeUsers = response
        }
      })
    )
  }
  private _subscribeToInboxChanges(){
    
  }

}
