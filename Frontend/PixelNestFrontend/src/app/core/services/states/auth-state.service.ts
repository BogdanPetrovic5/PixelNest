import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService {
  private IsAuthSuccessSubject = new BehaviorSubject<boolean | null>(false)
  isAuthSuccess$ = this.IsAuthSuccessSubject.asObservable()
  constructor() { }

  setIsAuthSuccess(isSuccess:boolean | null){
    this.IsAuthSuccessSubject.next(isSuccess)
  }
}
