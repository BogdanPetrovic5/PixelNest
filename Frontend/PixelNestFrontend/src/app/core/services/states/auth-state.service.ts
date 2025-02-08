import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService {
    private _isLoggedIn = new BehaviorSubject<boolean>(false);
    isLoggedIn$ = this._isLoggedIn.asObservable();

    setIsLoggedIn(value:boolean){
      this._isLoggedIn.next(value);
    }
}
