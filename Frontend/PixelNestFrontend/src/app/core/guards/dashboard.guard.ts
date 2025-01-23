import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { catchError, map, Observable, of } from 'rxjs';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DashboardGuard implements CanActivate {
  constructor(
 
    private _router:Router,
    private _authenticationService:AuthenticationService  
  ){

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      const excludedRoutes = ['/Authentication/Login', '/Authentication/Register']; // Add your login and register routes here
      if (excludedRoutes.includes(state.url)) {
        return true; 
      }
      return this._authenticationService.isLoggedIn().pipe(
        map(loggedIn => {
          if (loggedIn != null && loggedIn != undefined && loggedIn) {
            
            return true; 
          }else {
            this._router.navigate(['/Unauthorized']); 
            return false; 
          }
        }),
        catchError((error: HttpErrorResponse) => {
          this._router.navigate(['/Get Started']);
          return of(false); 
        })
      );
    }
  }
  

