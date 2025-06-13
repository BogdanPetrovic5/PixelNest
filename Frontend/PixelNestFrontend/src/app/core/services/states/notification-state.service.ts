import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NotificationDto } from '../../dto/notification.dto';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationStateService {
  private _newNotifictaion = new BehaviorSubject<boolean>(false)
  newNotification$ = this._newNotifictaion.asObservable()

  private _notificationType = new BehaviorSubject<string>("")
  _notificationType$ = this._notificationType.asObservable()

  private _notificationNumber = new BehaviorSubject<number>(0)
  notificationNumber$ = this._notificationNumber.asObservable()

  private _notifications = new BehaviorSubject<NotificationDto[]>([])
  notifications$ = this._notifications.asObservable();
  constructor(private _notificationService:NotificationService) { }

  loadNotifications(){
    this._notificationService.getNotifications().subscribe({
      next:response=>{
        this._notifications.next(response);
      }
    })
  }

  updateNotifications(notification:NotificationDto){
    let currentState = this._notifications.getValue();

    currentState = [notification, ...currentState]
    this._notifications.next(currentState)
  }
  setNewNotification(value:boolean){
    this._newNotifictaion.next(value)
  }
  setNotificationType(value:string){
    this._notificationType.next(value);
  }

  setNotificationNumber(value:number){
    this._notificationNumber.next(value);
  }
  updateNotification(value:number){
    let number = this._notificationNumber.getValue();
    if((number + value) >= 0) number += value;
    this._notificationNumber.next(number);
  }
}
