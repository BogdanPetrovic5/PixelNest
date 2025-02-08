import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';


@Component({
  selector: 'app-new-notification',
  templateUrl: './new-notification.component.html',
  styleUrls: ['./new-notification.component.scss']
})
export class NewNotificationComponent implements OnInit, OnDestroy{
    @Input() notification:string = "";
    subscribe!:Subscription;
    like:boolean = true;
    comment:boolean = false;
    follower:boolean = false;
    interval:any
    constructor(
      private _notificationState:NotificationStateService
    ){}
    ngOnDestroy(): void {
      this.subscribe.unsubscribe();
      clearInterval(this.interval)
    }
    ngOnInit(): void {
     
        this.subscribe = this._notificationState._notificationType$.subscribe({
          next:response=>{
            const stateMap: Record<string, keyof Pick<this, 'like' | 'comment' | 'follower'>> = {
              Like: 'like',
              Comment: 'comment',
              Follow: 'follower',
            };
        
            Object.keys(stateMap).forEach(key => this[stateMap[key]] = false); 
        
            if (response in stateMap) {
              this[stateMap[response]] = true;
        
              this.interval = setTimeout(() => {
                this[stateMap[response]] = false;
              }, 1500);
            }
          }
        })
      
     
    }
}
