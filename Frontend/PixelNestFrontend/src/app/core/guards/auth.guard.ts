import { Injectable } from '@angular/core';
// @ts-ignore: CanActivate is deprecated, but still functional for this project setup
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { UserSessionService } from '../services/user-session/user-session.service';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private _userSession:UserSessionService,
    private _router:Router,
    private _authenticationService:AuthenticationService  
  ){

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      return this._authenticationService.isLoggedIn().pipe(
        map(loggedIn => {
          if (loggedIn) {
            this._router.navigate(['/Dashboard/Feed']);
            return false; 
          }else {
            return true; 
          }
        }),
        catchError((error: HttpErrorResponse) => {
          return of(true); 
        })
      );
    }
    
  
  
}
