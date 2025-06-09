import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { finalize, Observable } from 'rxjs';
import { ApiTrackerService } from '../services/api-tracker/api-tracker.service';
import { Router } from '@angular/router';


@Injectable()
export class ApiTrackerInterceptor implements HttpInterceptor {

  constructor(
    private _apiTracker:ApiTrackerService,
    private _router:Router
  ) {}
  
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if(this._router.url.includes("/dashboard") || this._router.url.includes("/profile")){
      this._apiTracker.requestStarted();
    }
    return next.handle(request).pipe(
      finalize(() => {
        if(this._router.url.includes("/dashboard") || this._router.url.includes("/profile")) {
          this._apiTracker.requestEnded(); 
        }
      })
    );
  }
}
