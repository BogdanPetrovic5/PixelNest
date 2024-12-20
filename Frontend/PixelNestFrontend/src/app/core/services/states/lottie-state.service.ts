import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LottieStateService {
  private _isSuccessSubject = new BehaviorSubject<boolean>(false);
  public isSuccess$ = this._isSuccessSubject.asObservable();

  private _isInitializedSubject = new BehaviorSubject<boolean>(false);
  public isInitialized$ = this._isInitializedSubject.asObservable();

  constructor() { }

  setIsSuccess(value:boolean){
    this._isSuccessSubject.next(value)
  }

  setIsInitialized(value:boolean){
    this._isInitializedSubject.next(value);
  }
}
