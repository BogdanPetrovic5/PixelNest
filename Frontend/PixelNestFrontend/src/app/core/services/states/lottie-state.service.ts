import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LottieStateService {
  private _isSuccessSubject = new BehaviorSubject<boolean>(false);
  public isSuccess$ = this._isSuccessSubject.asObservable();
  constructor() { }

  setIsSuccess(value:boolean){
    this._isSuccessSubject.next(value)
  }
}
