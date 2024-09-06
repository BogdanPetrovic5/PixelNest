import { Component } from '@angular/core';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  public selectedTab:number | null = 1;

  constructor(private _dashboardStateMenagment:DashboardStateService){

  }

  openNewPostDialog(){
    this._dashboardStateMenagment.setIsTabSelected(true);
  }

  changeTab(tabIndex:number){
    this.selectedTab = tabIndex
  }
}
