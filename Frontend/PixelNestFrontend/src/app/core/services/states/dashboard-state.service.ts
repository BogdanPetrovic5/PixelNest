import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardStateService {
  private NewPostTabSubject = new BehaviorSubject<boolean>(false);
  newPostTab$ = this.NewPostTabSubject.asObservable()

  private IsLikesTabSubject = new BehaviorSubject<boolean>(false)
  isLikesTab$ = this.IsLikesTabSubject.asObservable()

  private NewLocationSubject = new BehaviorSubject<string | null>(null)
  location$ = this.NewLocationSubject.asObservable()

  private _newStoryTab = new BehaviorSubject<boolean>(false)
  newStoryTab$ = this._newStoryTab.asObservable();

  private _storyPreview = new BehaviorSubject<boolean>(false)
  storyPreview$ = this._storyPreview.asObservable();

  private _deleteDialog = new BehaviorSubject<boolean>(false)
  deleteDialog$ = this._deleteDialog.asObservable();

  private _notification = new BehaviorSubject<boolean>(false)
  notification$ = this._notification.asObservable();

  private _notificationSender = new BehaviorSubject<string>("")
  notificationSender$ = this._notificationSender.asObservable();
  
  private _notificationChatID = new BehaviorSubject<string>("")
  notificationChatID$ = this._notificationChatID.asObservable();
  
  private _notificationMessage = new BehaviorSubject<string>("")
  notificationMessage$ = this._notificationMessage.asObservable();

  private _sessionExpiredDialog = new BehaviorSubject<boolean>(false)
  sessionExpiredDialog$ = this._sessionExpiredDialog.asObservable();

  private _scrollPosition = new BehaviorSubject<number>(0)
  scrollPosition = this._scrollPosition.asObservable();


  private _isPostApiFinished = new BehaviorSubject<boolean>(false)
  isPostApiFinished = this._isPostApiFinished.asObservable();
  private _isStoryApiFinished = new BehaviorSubject<boolean>(false)
  isStoryApiFinished = this._isStoryApiFinished.asObservable();

  
  constructor() { }

  setIsPostApiFinished(value:boolean){
    this._isPostApiFinished.next(value)
  }
  setIsStoryApiFinished(value:boolean){
    this._isStoryApiFinished.next(value)
  }


  setNewScrollPosition(value:number){
    this._scrollPosition.next(value);
  }
  getScrollPosition() : number{
    return this._scrollPosition.getValue()
  }
  setIsNewStoryTabOpen(isSelected:boolean){
    this._newStoryTab.next(isSelected);
  }

  setIsTabSelected(isSelected:boolean){
    this.NewPostTabSubject.next(isSelected)
  }
  setIsLikesTab(isSelected:boolean){
    this.IsLikesTabSubject.next(isSelected);
  }

  setNewLocation(location:string){
    this.NewLocationSubject.next(location);
  }
  setStoryPrewiew(value:boolean){
    this._storyPreview.next(value);
  }

  setDeleteDialog(value:boolean){
    this._deleteDialog.next(value);
  }
  setIsNotification(value:boolean){
    this._notification.next(value);
  }
  setMessage(value:string){
    this._notificationMessage.next(value)
  }
  setSender(value:string){
    this._notificationSender.next(value);
  }
  setChatID(value:string){
    this._notificationChatID.next(value);
  }
  setSessionExpiredDialog(value:boolean){
    this._sessionExpiredDialog.next(value);
  }
}
