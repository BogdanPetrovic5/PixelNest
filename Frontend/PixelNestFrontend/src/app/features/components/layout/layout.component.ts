import { Component, OnDestroy, OnInit } from '@angular/core';
import {Subject, Subscription, takeUntil } from 'rxjs';
import { LayoutStateInterface } from 'src/app/core/dto/layoutState.interface';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { LayoutFacadeService } from 'src/app/core/services/states/facade/layout-facade.service';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { WebsocketService } from 'src/app/core/services/websockets/websocket.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  
})
export class LayoutComponent implements OnInit, OnDestroy{
  private _destroy$ = new Subject<void>();
  newPost:boolean | null = false;
  subscriptions: Subscription = new Subscription();

  isSuccess:boolean = false
  isLoading:boolean = false;
  deleteDialog:boolean = false;
  logOutDialog:boolean = false;
  isNotification:boolean = false;
  sessionExpired:boolean = false;
  isFeedInitilizing:boolean = false

  layoutState$!: LayoutStateInterface;
  constructor(
    private _dashboardStateManagement:DashboardStateService,
    private _userSession:UserSessionService,
    private _websocketService:WebsocketService,
    private _chatState:ChatStateService,
    private _notificationState:NotificationStateService,
    private _postState:PostStateService,
    private _authState:AuthStateService,
    private _layoutFacade:LayoutFacadeService
  ){
   
  }
 
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
    this._websocketService.close();
    this._chatState.resetNewMessages()
    this._notificationState.setNotificationNumber(0);
    this._postState.feedCurrentPage = 1;
    this._postState.setPosts([])
    this._postState.resetFeed([])
    this._authState.setIsLoggedIn(false);
    this._dashboardStateManagement.setNewScrollPosition(0)
    this._destroy$.next();
    this._destroy$.complete();
  }
  ngOnInit(): void {
    this._initSubscriptions();
    this._websocketService.connect()
  }

  private _initSubscriptions(): void {
    this._proccessLastSenderIDS();
    this._userSession.setTokenExpiration(this._userSession.getFromCookie("tokenExpirationAt"))
    this._userSession.monitorUserActivity();

    this._layoutFacade.state$.pipe(
        takeUntil(this._destroy$)
      )
      .subscribe({
        next:state=>{
          this.layoutState$ = state;
        }
    })

  }
  
  private _proccessLastSenderIDS(){
    let ids = this._userSession.getFromCookie("ids")
    if(ids){
      let parsed = JSON.parse(ids)
      
      this._chatState.setLastIDS(parsed);
    }
  }
  
}
