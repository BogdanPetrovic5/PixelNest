import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiTrackerService {
  private _activeRequests = 0;
  private _requestsCompleted = new BehaviorSubject<boolean>(false);
  requestCompleted = this._requestsCompleted.asObservable()
  
  constructor() { }

  requestStarted(): void {
   if(this._activeRequests >= 3){
      this._requestsCompleted.next(true);
   }
    this._activeRequests++;
    console.log(this._activeRequests)
  }
  requestEnded(): void {
    this._activeRequests--;
    console.log(this._activeRequests)
    if (this._activeRequests === 0) {
      this._requestsCompleted.next(false); 
    }
  }
}
