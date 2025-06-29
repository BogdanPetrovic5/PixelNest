import { Component, OnInit } from '@angular/core';
import { NotificationDto } from 'src/app/core/dto/notification.dto';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';

@Component({
  selector: 'app-notifications-widget',
  templateUrl: './notifications-widget.component.html',
  styleUrls: ['./notifications-widget.component.scss']
})
export class NotificationsWidgetComponent implements OnInit{
  notifications:NotificationDto[] = []
  lastTwoNotifications:NotificationDto[] = []
  constructor(private _notificationState:NotificationStateService){
    
  }
  ngOnInit(): void {
    this._notificationState.loadNotifications();
    this._initializeComponent()
  }

  private _initializeComponent(){
    this._notificationState.notifications$.subscribe({
      next:response=>{
        this.notifications = response;
       
        this.lastTwoNotifications = response.slice(0,2);
      }
    })
  }
}
