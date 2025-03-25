import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class GlobalInterceptor implements HttpInterceptor {

  constructor(private _router:Router) {}
  
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
  
    const authorizationReq = request.clone({
      withCredentials:true
    })
    return next.handle(authorizationReq)
    
  }
}
