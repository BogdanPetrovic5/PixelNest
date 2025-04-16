import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Subscription } from 'rxjs';
import { CustomRouteReuseStrategy } from 'src/app/core/route-reuse-strategy';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-authentication-page',
  templateUrl: './authentication-page.component.html',
  styleUrls: ['./authentication-page.component.scss']
})
export class AuthenticationPageComponent implements OnInit{

  defaultRoute:string = 'Register'

  isSuccess:boolean = false;
  warning:boolean = false;
  isInitialized:boolean = false;

  subscriptions: Subscription = new Subscription();

  
  constructor(
    private _route:ActivatedRoute,
    private _userSession:UserSessionService,
    private _lottieState:LottieStateService,
    private _routeReuse:CustomRouteReuseStrategy,
    private _router:Router
  ){}

  ngOnInit():void{
    if(!this._router.url.includes("/Redirect-Page")) this.warning = true;
    this._routeReuse.destroyComponents();
    this.subscriptions.add(
      this._lottieState.isSuccess$.subscribe({
        next:response=>{
          this.isSuccess = response;
        }
      })
    )
    this.subscriptions.add(
      this._lottieState.isInitialized$.subscribe({
        next:response=>{
          this.isInitialized = response;
        }
      })
    )
    
  }
  close(){
    
    this.warning = false;
  }
}
