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

  private _notificationMessage = new BehaviorSubject<string>("")
  notificationMessage$ = this._notificationMessage.asObservable();

  private _sessionExpiredDialog = new BehaviorSubject<boolean>(false)
  sessionExpiredDialog$ = this._sessionExpiredDialog.asObservable();
  constructor() { }

  setIsNewStoryTabOpen(isSelected:boolean){
    this._newStoryTab.next(isSelected);
  }

  setIsTabSelected(isSelected:boolean | null){
    this.NewPostTabSubject.next(isSelected)
  }
  setIsLikesTab(isSelected:boolean | null){
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
  setSessionExpiredDialog(value:boolean){
    this._sessionExpiredDialog.next(value);
  }
}
