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
      const excludedRoutes = ['/authentication/login', '/authentication/register']; 
      if (excludedRoutes.includes(state.url)) {
        return true; 
      }
      return this._authenticationService.isLoggedIn().pipe(
        map(loggedIn => {
          if (loggedIn != null && loggedIn != undefined && loggedIn) {
            
            return true; 
          }else {
            this._router.navigate(['/unauthorized']); 
            return false; 
          }
        }),
        catchError((error: HttpErrorResponse) => {
          this._router.navigate(['/get started']);
          return of(false); 
        })
      );
    }
  }
  

