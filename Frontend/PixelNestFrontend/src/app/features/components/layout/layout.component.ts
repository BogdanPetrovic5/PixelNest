import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
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
    private _dashboardStateMenagment:DashboardStateService,
    private _lottieState:LottieStateService,
    private _userSession:UserSessionService,
    private _websocketService:WebsocketService
  ){
    
  }
  ngOnDestroy(): void {
    this._websocketService.close()
  }
  ngOnInit(): void {
    this.subscriptions.add(
      this._dashboardStateMenagment.newPostTab$.subscribe(response =>{
        if(this.newPost != null && response != null){
          this.newPost = response
        }
      })
    )
    this.subscriptions.add(
      this._lottieState.isSuccess$.subscribe({
        next:response=>{
          this.isSuccess = response
        }
      })
    )
    this.subscriptions.add(
      this._lottieState.isInitialized$.subscribe({
        next:response=>{
          this.isLoading =response;
        }
      })
    )
    this.subscriptions.add(
      this._userSession.logOutDialog$.subscribe({
        next:response=>{
          this.logOutDialog = response;
        }
      })
    )
    this.subscriptions.add(
      this._dashboardStateMenagment.notification$.subscribe({
        next:response=>{
          this.isNotification = response;
          setTimeout(()=>{
            this.isNotification = false;
          }, 1300)
        }
      })
    )
    this._websocketService.connect(this._userSession.getFromCookie("username"))
  }
  
}
