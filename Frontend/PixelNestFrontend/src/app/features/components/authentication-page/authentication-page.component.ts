import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Subscription } from 'rxjs';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-authentication-page',
  templateUrl: './authentication-page.component.html',
  styleUrls: ['./authentication-page.component.scss']
})
export class AuthenticationPageComponent implements OnInit{
  subscriptions: Subscription[] = [];
  defaultRoute:string = 'Register'
  constructor(
    private _route:ActivatedRoute,
    private _userSession:UserSessionService
  ){}

  ngOnInit():void{
    this._userSession.clearCookies()
  }

}
