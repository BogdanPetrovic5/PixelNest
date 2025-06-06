import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.scss']
})
export class TopNavigationComponent implements OnInit, OnDestroy{
    newNotification:boolean = false;
    subscribe:Subscription = new Subscription();
    interval:any
    constructor(
      private _router:Router,
      private _notification:NotificationStateService
    ){}
    ngOnDestroy(): void {
      this.subscribe.unsubscribe()
      
    }

    
    ngOnInit(): void {
      this.subscribe.add(
        this._notification.newNotification$.subscribe({
          next:response=>{
            this.newNotification = response
  
           this.interval = setTimeout(()=>{
              this._notification.setNewNotification(false);
              clearTimeout(this.interval)
            }, 1500)
          }
        })
      )
      
    }
}
