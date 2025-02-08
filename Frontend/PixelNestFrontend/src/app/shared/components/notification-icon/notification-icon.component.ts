import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter, Subject, Subscription, takeUntil } from 'rxjs';
import { NotificationService } from 'src/app/core/services/notification/notification.service';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-notification-icon',
  templateUrl: './notification-icon.component.html',
  styleUrls: ['./notification-icon.component.scss']
})
export class NotificationIconComponent {
private destroy$ = new Subject<void>();
private routerSubscription = new Subscription()
  constructor(
    private _router:Router,
    private _notificationState:NotificationStateService,
    private _notificationService:NotificationService,
    private _userSession:UserSessionService
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
    
     this._userSession.currentUrl$.subscribe({
      next:resposne=>{
        if(resposne == 'Feed'){
        
        }
      }
     })
     this.countNotifications()
     this.routerSubscription = this._router.events
     .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
     .subscribe((event:NavigationEnd)=>{
        console.log(event.url)
        if(event.url.includes('/Dashboard')){
          
          this.countNotifications()
        }
     })
      
    }
    countNotifications(){
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
