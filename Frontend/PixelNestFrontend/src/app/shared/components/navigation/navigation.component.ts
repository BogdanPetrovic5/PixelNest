
import {  Component, OnInit } from '@angular/core';
import {  NavigationEnd, Router } from '@angular/router';
import { filter, Subscription } from 'rxjs';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { NotificationStateService } from 'src/app/core/services/states/notification-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit{
  public selectedTab!:number
  public clientGuid:string = '';
  public username:string = ''
  subscription:Subscription = new Subscription();
  newNotification:boolean = true;
  
  interval:any
  stringUrl = ""
  routeToTabMap: { [key: string]: number } = {
  };
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _router:Router,
    private _notification:NotificationStateService
  ){

  }
  ngOnInit():void{
    this.clientGuid = this._userSessionService.getFromCookie("sessionID");
    this.username = this._userSessionService.getFromCookie("username");
    this.routeToTabMap = {
      'dashboard/feed': 1,
      [`profile/${this.clientGuid}/${this.username}`]: 4,
      'messages': 2,
      'notifications': 3,
      'search': 5
    };
    this.subscription.add(
      this._notification.newNotification$.subscribe({
        next:response=>{
          this.newNotification = response
        }
      })
    )
    this._router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
  
      )
      .subscribe((response:any) => {
        this.setActiveTab(this._router.url);
        console.log("Subscribed: ", this._router.url)
    })
    
    this.setActiveTab(this._router.url);
    
  }

  setActiveTab(url:string){
    console.log("set active tab: ", url);
    this.selectedTab = Object.keys(this.routeToTabMap).find(key => url.includes(key)) 
    ? this.routeToTabMap[Object.keys(this.routeToTabMap).find(key => url.includes(key))!] : 0
   
  }

  navigateToUserProfile(){
   
    this._router.navigate([`/profile/${this.clientGuid}/${this.username}`])
  }

  navigate(route:string){
    this._router.navigate([route])
  }

  openNewPostDialog(){
    this._dashboardStateMenagment.setIsTabSelected(true);
  }

  changeTab(tabIndex:number){
    this.selectedTab = tabIndex


  }
  logout(){
    this._userSessionService.setLogOutDialog(true)
  }
 
}
