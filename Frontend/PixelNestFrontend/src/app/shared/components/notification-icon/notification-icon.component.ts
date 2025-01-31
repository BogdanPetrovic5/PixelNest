import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';

@Component({
  selector: 'app-notification-icon',
  templateUrl: './notification-icon.component.html',
  styleUrls: ['./notification-icon.component.scss']
})
export class NotificationIconComponent {
private destroy$ = new Subject<void>();
  constructor(
    private _router:Router,
    private _notificationState:NotificationStateService,
    private _notificationService:NotificationService
  ){}

    newNotifications:number = 0;
    navigate(route:string){
      this._router.navigate([`/${route}`])
    }
    ngOnInit(): void {
      this._notificationState.notificationNumber$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          
          this.newNotifications = response;
        },
        error: err => {
          console.error('Error updating new messages:', err);
        },
      });

     
      this._notificationService.countNotifications().subscribe(
        {
          next:response=>{
            this.newNotifications = response;
            this._notificationState.setNotificationNumber(this.newNotifications)
          }
        }
      )
    }
    ngOnDestroy(): void {
      
      this.destroy$.complete(); 
    }
}
