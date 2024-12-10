import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoryStateService {
  private _currentStory = new BehaviorSubject<number>(0);
  currentStory$ = this._currentStory.asObservable();

  private _currentPage = new BehaviorSubject<number>(1);
  currentPage$ = this._currentPage.asObservable();


  resetCurrentState(){
    this._currentStory.next(0);
  }

  setCurrentStoryState(value:number){
    const currentValue = this._currentStory.getValue();
    this._currentStory.next(currentValue + value);
  }
  setCurrentPageState(value:number){
    const currentValue = this._currentPage.getValue();
    this._currentPage.next(currentValue + value);
  }
  constructor() { }
}
