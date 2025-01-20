 import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { StoriesDto } from '../../dto/stories.dto';
import { StoryService } from '../story/story.service';

@Injectable({
  providedIn: 'root'
})
export class StoryStateService {

  private _currentStory = new BehaviorSubject<number>(0);
  currentStory$ = this._currentStory.asObservable();

  private _currentPage = new BehaviorSubject<number>(1);
  currentPage$ = this._currentPage.asObservable();

  private _storyViewerList = new BehaviorSubject<boolean>(false);
  isViewerList$ = this._storyViewerList.asObservable();

  private _viewerListAnim = new BehaviorSubject<boolean>(false);
  viewerListAnim$ = this._viewerListAnim.asObservable();


  private _storyList = new BehaviorSubject<StoriesDto[]>([]);
  stories$ = this._storyList.asObservable();


  private _storyListByUser = new BehaviorSubject<StoriesDto[]>([])
  storyListByUser$ = this._storyListByUser.asObservable();
  
  public stories:StoriesDto[] = []
  public storiesByUser:StoriesDto[] = []

  constructor(private _storyService:StoryService) { }
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

  setIsViewerList(value:boolean){
    this._storyViewerList.next(value);
  }
  getViewerListState():boolean{
    return this._storyViewerList.getValue();
  }
  setViewerListAnim(value:boolean){
    this._viewerListAnim.next(value);
  }
  setStories(){
    this._storyList.next([])
    this._storyListByUser.next([]);
  }
  fetchCurrentUserStories(username:string, forCurrentUser:boolean){
    this._storyService.getStories(username, forCurrentUser).subscribe({
      next:response=>{
        this.storiesByUser = response;
        this._storyListByUser.next(this.storiesByUser)
      }
    })
  }
  fetchStories(username:string, forCurrentUser:boolean){
    this._storyService.getStories(username, forCurrentUser).subscribe({
      next:response=>{
        this.stories = response;
        this._storyList.next(this.stories)
      }
    })
  }
  
}
