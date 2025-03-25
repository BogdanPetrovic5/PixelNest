import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { CustomRouteReuseStrategy } from '../route-reuse-strategy';

@Injectable()
export class UnauthorizedAccessInterceptor implements HttpInterceptor {

  constructor(private _router:Router, private _routeReuse:CustomRouteReuseStrategy) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const excludedUrls = ['/api/Authentication/IsLoggedIn', '/api/Authentication/GetLoginResponse'];
    if (excludedUrls.some((url) => request.url.includes(url))) {
      return next.handle(request); 
    }
    return next.handle(request).pipe(
          catchError(error => {
            if (error.status === 401) {
                
                this._router.navigate(['/Unauthorize'])
            }
            return throwError(error);
          })
    );
  }
}
