import { Component, OnInit } from '@angular/core';
import { Subscriber, Subscription } from 'rxjs';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit{
  sender:string =""
  message:string =""
  subscription:Subscription = new Subscription();
  constructor(private _dashboardState:DashboardStateService){}
  ngOnInit(): void {
    this.subscription.add(
      this._dashboardState.notificationMessage$.subscribe({
        next:response=>{
          this.message = response
        }
      })
    )
    this.subscription.add(
      this._dashboardState.notificationSender$.subscribe({
        next:response=>{
          this.sender = response
        }
      })
    )
  }
}
