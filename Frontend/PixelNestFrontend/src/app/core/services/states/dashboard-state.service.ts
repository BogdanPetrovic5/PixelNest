import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardStateService {
  private NewPostTabSubject = new BehaviorSubject<boolean | null>(false);
  newPostTab$ = this.NewPostTabSubject.asObservable()

  private IsLikesTabSubject = new BehaviorSubject<boolean | null>(false)
  isLikesTab$ = this.IsLikesTabSubject.asObservable()
  constructor() { }

  setIsTabSelected(isSelected:boolean | null){
    this.NewPostTabSubject.next(isSelected)
  }
  setIsLikesTab(isSelected:boolean | null){
    this.IsLikesTabSubject.next(isSelected);
  }
}
