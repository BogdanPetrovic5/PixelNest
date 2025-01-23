import { Component } from '@angular/core';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-session-expired-dialog',
  templateUrl: './session-expired-dialog.component.html',
  styleUrls: ['./session-expired-dialog.component.scss']
})
export class SessionExpiredDialogComponent {
  constructor(
    private _userSession:UserSessionService,
    private _userService:UserService,
    private _authService:AuthenticationService,
    private _dashboardState:DashboardStateService
  ){}

  choice(option:string){
    if(option === 'yes'){
      this._userSession.refreshToken();
      
    }else this._authService.logout("");
    this._dashboardState.setSessionExpiredDialog(false);
  }
}
