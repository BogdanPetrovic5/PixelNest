import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Subscription } from 'rxjs';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-authentication-page',
  templateUrl: './authentication-page.component.html',
  styleUrls: ['./authentication-page.component.scss']
})
export class AuthenticationPageComponent implements OnInit{

  defaultRoute:string = 'Register'

  isSuccess:boolean = false;

  subscriptions: Subscription = new Subscription();

  constructor(
    private _route:ActivatedRoute,
    private _userSession:UserSessionService,
    private _authState:AuthStateService
  ){}

  ngOnInit():void{


    this.subscriptions.add(
      this._authState.isAuthSuccess$.subscribe(response=>{
        if(response) this.isSuccess = response;
      })
    )
  }

}
