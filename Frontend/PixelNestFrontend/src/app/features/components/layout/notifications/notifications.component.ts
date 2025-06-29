import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationDto } from 'src/app/core/dto/notification.dto';
import { NotificationOpened } from 'src/app/core/dto/notificationOpened.dto';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit{
  baseUrl:string = ""
  notifications:NotificationDto[] = []
  notificationsID:NotificationOpened = {
    notificationID:[]
  }
  constructor(
    private _notificationService:NotificationService,
    private _router:Router,
    private _notificationState:NotificationStateService
  ){

  }

  ngOnInit(): void {
   
     this.baseUrl = environment.blobStorageBaseUrl;
    this._notificationState.notifications$.subscribe({
      next:response=>{
        this.notifications = response;
     
        this._openNotifications();
        
      }
    })
  
  }

  navigation(url:string){
    this._router.navigate([url]);
  }
  private _openNotifications(){
    for(let i = 0; i< this.notifications.length;i++){
      this.notificationsID.notificationID.push(this.notifications[i].notificationID)
    }
    
    this._notificationService.markAsOpened(this.notificationsID).subscribe({
      next:response=>{
      
      }
    })

  }
}
