import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfileStateService {
  private _currentProfileUrl = new BehaviorSubject<string>("/assets/images/user.png")
 currentProfileUrl$ = this._currentProfileUrl.asObservable();


  constructor() { }

  setCurrentUrl(value:string){
    this._currentProfileUrl.next(value);
  }
}
