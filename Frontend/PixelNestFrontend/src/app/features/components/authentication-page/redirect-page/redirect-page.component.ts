import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GoogleAuthenticationService } from 'src/app/core/services/authentication/google/google-authentication.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-redirect-page',
  templateUrl: './redirect-page.component.html',
  styleUrls: ['./redirect-page.component.scss']
})
export class RedirectPageComponent implements OnInit{
  constructor(
    private _googleAuth:GoogleAuthenticationService,
    private _userSession:UserSessionService,
    private _router:Router
  ){

  }
  ngOnInit(): void {
    const state = this._userSession.getFromLocalStorage("state");
    this._googleAuth.getGoogleResponse(state).subscribe({
      next:response=>{
        this._userSession.setToCookie("username", response.username);
        this._userSession.setToCookie("email", response.email);
        this._userSession.setToCookie("userID", response.clientGuid);
        this._router.navigate(['dashboard/feed'])
        
      }
    })
  }
    
}
