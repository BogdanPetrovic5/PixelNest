import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-log-out-dialog',
  templateUrl: './log-out-dialog.component.html',
  styleUrls: ['./log-out-dialog.component.scss']
})
export class LogOutDialogComponent {
  anim:boolean = false
  constructor(
    private _userSession:UserSessionService,
    private _authService:AuthenticationService,
    private _router:Router,
    private _authState:AuthStateService
  ){

  }
  choose(choice:'yes'| 'no'){
    if(choice == 'yes'){
      let email = this._userSession.getFromCookie("email");
      this._authService.logout(email).subscribe(response =>{
        
        this._userSession.clearCookies();
        this._userSession.clearStorage();
        this._userSession.setLogOutDialog(false);
         
        this._router.navigate(["/Authentication/Login"])
      
        this._authState.setIsLoggedIn(false);
      
      
      },(error:HttpErrorResponse)=>{
        console.log(error)
      })
    }else this.close();
  }
  
  close(){

    this.anim = true;
    setTimeout(()=>{
      this._userSession.setLogOutDialog(false)
      this.anim = false;
    })
  }
}
