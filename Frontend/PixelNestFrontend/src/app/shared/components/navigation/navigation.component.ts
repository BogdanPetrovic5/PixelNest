import { Component } from '@angular/core';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  public selectedTab:number | null = 1;
  changeTab(tabIndex:number){
    this.selectedTab = tabIndex
  }
}
