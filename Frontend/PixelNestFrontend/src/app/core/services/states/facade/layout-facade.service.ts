import { Injectable } from '@angular/core';
import { DashboardStateService } from '../dashboard-state.service';
import { LottieStateService } from '../lottie-state.service';
import { ApiTrackerService } from '../../api-tracker/api-tracker.service';
import { CacheService } from '../../cache/cache.service';
import { UserSessionService } from '../../user-session/user-session.service';
import { WebsocketService } from '../../websockets/websocket.service';
import { AuthStateService } from '../auth-state.service';
import { ChatStateService } from '../chat-state.service';
import { NotificationStateService } from '../notification-state.service';
import { PostStateService } from '../post-state.service';
import { combineLatest, map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LayoutFacadeService {

  readonly state$ = combineLatest([
    this._dashboardState.newPostTab$,
    this._lottieState.isSuccess$,
    this._lottieState.isInitialized$,
    this._userSession.logOutDialog$,
    this._dashboardState.notification$.pipe((
      tap((respone)=>{
        if(respone) this._dashboardState.setIsNotification(false);
      })
    )),
    this._dashboardState.sessionExpiredDialog$,
    this._apiTracker.requestCompleted,
    this._cacheService.checkCache().pipe(
      tap((response) => {
        if (response) {
          this._cacheService.setCacheState(response);
        }
      })
    )
  ]).pipe(
    map(
      (
        [newPost, isSuccess, isInitializing, logOut, notification, sessionExpired, feedInit, cache]
      ) =>(
        {
          isNewPostTabOpened:newPost ?? false,
          isSuccess,
          isInitializing,
          isLogoutDialogOpened: logOut ?? false,
          newNotification:notification ?? false,
          isSessionExpired: sessionExpired ?? false,
          isFeedInitializing:feedInit ?? false,
          hasCacheChanged: cache ?? false
        }
      )
    )
  )


  constructor(
    private _dashboardState:DashboardStateService,
    private _lottieState:LottieStateService,
    private _userSession:UserSessionService,
    private _cacheService:CacheService,
    private _apiTracker:ApiTrackerService
  ) { }


}
