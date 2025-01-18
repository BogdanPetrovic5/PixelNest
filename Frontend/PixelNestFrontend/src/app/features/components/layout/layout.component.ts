import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ChatService } from 'src/app/core/services/chat/chat.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { WebsocketService } from 'src/app/core/services/websockets/websocket.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy{
  newPost:boolean | null = false;
  subscriptions: Subscription = new Subscription();

  isSuccess:boolean = false
  isLoading:boolean = false;
  deleteDialog:boolean = false;
  logOutDialog:boolean = false;
  isNotification:boolean = false;

  
  constructor(
    private _dashboardStateManagement:DashboardStateService,
    private _lottieState:LottieStateService,
    private _userSession:UserSessionService,
    private _websocketService:WebsocketService,
    private _chatService:ChatService,
    private _chatState:ChatStateService
  ){
    
  }
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
   
    this._websocketService.close();
    
  }
  ngOnInit(): void {
    this._initSubscriptions();
    this._websocketService.connect(this._userSession.getFromCookie('username'))
  }

  private _initSubscriptions(): void {
    this._proccessLastSenderIDS();
    const subscriptionsList = [
      {
        observable$: this._dashboardStateManagement.newPostTab$,
        handler: (response: any) => {
          if (this.newPost != null && response != null) {
            this.newPost = response;
          }
        },
      },
      {
        observable$: this._lottieState.isSuccess$,
        handler: (response: boolean | null) => {
          this.isSuccess = response ?? false;
        },
      },
      {
        observable$: this._lottieState.isInitialized$,
        handler: (response: boolean | null) => {
          this.isLoading = response ?? false; 
        },
      },
      {
        observable$: this._userSession.logOutDialog$,
        handler: (response: boolean | null) => {
          this.logOutDialog = response ?? false; 
        },
      },
      {
        observable$: this._dashboardStateManagement.notification$,
        handler: (response: boolean | null) => {
          this.isNotification = response ?? false; 
          if (this.isNotification) {
            setTimeout(() => {
              this._dashboardStateManagement.setIsNotification(false)
            }, 1700);
          }
        },
      },
    ];
  
    subscriptionsList.forEach(({ observable$, handler }) => {
      this.subscriptions.add(observable$.subscribe(handler));
    });
  }
  
  private _proccessLastSenderIDS(){
    let ids = this._userSession.getFromCookie("ids")
    if(ids){
      let parsed = JSON.parse(ids)
      
      this._chatState.setLastIDS(parsed);
    }
    
    
  }
  
}
