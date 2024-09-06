import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardStateService {
  private NewPostTabSubject = new BehaviorSubject<boolean | null>(false);
  newPostTab$ = this.NewPostTabSubject.asObservable()

  constructor() { }

  setIsTabSelected(isSelected:boolean | null){
    this.NewPostTabSubject.next(isSelected)
  }
}
