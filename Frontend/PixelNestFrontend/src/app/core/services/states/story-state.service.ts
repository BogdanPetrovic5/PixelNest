import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoryStateService {
  private _currentStory = new BehaviorSubject<number>(0);
  currentStory$ = this._currentStory.asObservable();

  resetCurrentState(){
    this._currentStory.next(0);
  }

  setCurrentStoryState(value:number){
    const currentValue = this._currentStory.getValue();
    this._currentStory.next(currentValue + value);
  }
  constructor() { }
}
