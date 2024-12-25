import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit{
  newPost:boolean | null = false;
  subscriptions: Subscription = new Subscription();

  isSuccess:boolean = false
  isLoading:boolean = false;
  deleteDialog:boolean = false;
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _lottieState:LottieStateService
  ){
    
  }
  ngOnInit(): void {
    this.subscriptions.add(
      this._dashboardStateMenagment.newPostTab$.subscribe(response =>{
        if(this.newPost != null && response != null){
          this.newPost = response
        }
      })
    )
    this.subscriptions.add(
      this._lottieState.isSuccess$.subscribe({
        next:response=>{
          this.isSuccess = response
        }
      })
    )
    this.subscriptions.add(
      this._lottieState.isInitialized$.subscribe({
        next:response=>{
          this.isLoading =response;
        }
      })
    )
  
  }
  
}
