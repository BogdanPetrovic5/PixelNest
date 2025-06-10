import { AfterContentInit, AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CustomRouteReuseStrategy } from 'src/app/core/route-reuse-strategy';
import { ApiTrackerService } from 'src/app/core/services/api-tracker/api-tracker.service';
import { CacheService } from 'src/app/core/services/cache/cache.service';
import { ChatService } from 'src/app/core/services/chat/chat.service';
import { IndexedDbService } from 'src/app/core/services/indexed-db/indexed-db.service';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { WebsocketService } from 'src/app/core/services/websockets/websocket.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  
})
export class LayoutComponent implements OnInit, OnDestroy, AfterContentInit{
  newPost:boolean | null = false;
  subscriptions: Subscription = new Subscription();

  isSuccess:boolean = false
  isLoading:boolean = false;
  deleteDialog:boolean = false;
  logOutDialog:boolean = false;
  isNotification:boolean = false;
  sessionExpired:boolean = false;
  isFeedInitilizing:boolean = false

  constructor(
    private _dashboardStateManagement:DashboardStateService,
    private _lottieState:LottieStateService,
    private _userSession:UserSessionService,
    private _websocketService:WebsocketService,
    private _chatState:ChatStateService,
    private _notificationState:NotificationStateService,
    private _cacheService:CacheService,
    private _postState:PostStateService,
    private _authState:AuthStateService,
    private _cdr:ChangeDetectorRef,
    private _apiTracker:ApiTrackerService
  ){
   
  }
  ngAfterContentInit(): void {
    
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
  }
  ngOnInit(): void {
    this._cdr.detectChanges()
   
   
    this._initSubscriptions();
    this._websocketService.connect()
  }

  private _initSubscriptions(): void {
    this._proccessLastSenderIDS();
    this._userSession.setTokenExpiration(this._userSession.getFromCookie("tokenExpirationAt"))
    this._userSession.monitorUserActivity();
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
      {
        observable$: this._dashboardStateManagement.sessionExpiredDialog$,
        handler: (response: boolean | null) =>{
          this.sessionExpired = response ?? false
          
        }
      },
      {
        observable$: this._cacheService.checkCache(),
        handler: (response: boolean | null) =>{
          if(response) this._cacheService.setCacheState(response);
         
        
        }
      },
      {
        observable$: this._apiTracker.requestCompleted,
        handler: (response: boolean | null) =>{
       
          if(response != null) this.isFeedInitilizing = response
         
        
        }
      }
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
