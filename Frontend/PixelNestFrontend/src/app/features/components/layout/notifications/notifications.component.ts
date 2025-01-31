import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationDto } from 'src/app/core/dto/notification.dto';
import { NotificationOpened } from 'src/app/core/dto/notificationOpened.dto';
import { NotificationService } from 'src/app/core/services/notification/notification.service';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit{

  notifications:NotificationDto[] = []
  notificationsID:NotificationOpened = {
    notificationID:[]
  }
  constructor(
    private _notificationService:NotificationService,
    private _router:Router
  ){

  }

  ngOnInit(): void {
    this._notificationService.getNotifications().subscribe({
      next:response=>{
        this.notifications = response;
        console.log(this.notifications)
        for(let i = 0; i< this.notifications.length;i++){
          this.notificationsID.notificationID.push(this.notifications[i].notificationID)
        }
        console.log(this.notificationsID.notificationID)
        this._notificationService.markAsOpened(this.notificationsID).subscribe({
          next:response=>{
            console.log(response)
          }
        })
        console.log(this.notifications)
      }
    })
  }
  navigation(url:string){
    this._router.navigate([url]);
  }
}
