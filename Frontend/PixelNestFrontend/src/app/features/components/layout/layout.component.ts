import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit{
  newPost:boolean | null = false;
  subscriptions: Subscription = new Subscription();

  constructor(private _dashboardStateMenagment:DashboardStateService){
    
  }
  ngOnInit(): void {
    this.subscriptions.add(
      this._dashboardStateMenagment.newPostTab$.subscribe(response =>{
        if(this.newPost != null && response != null){
          this.newPost = response
        }
      })
    )
  }
  
}
