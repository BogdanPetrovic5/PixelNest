import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit{
  public selectedTab!:number
  public username:string = this._userSessionService.getFromCookie("username")

  routeToTabMap: { [key: string]: number } = {
    'Dashboard/Feed': 1,
    ["Profile/"+this.username]: 4,
    'Messages': 2,
    'Notifications': 3,
    'Search': 5

  };
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _authService:AuthenticationService,
    private _router:Router,
    private cdr: ChangeDetectorRef,
    private _route:ActivatedRoute
  ){

  }
  ngOnInit():void{
    this._router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
  
      )
      .subscribe((response:any) => {
        this.setActiveTab(this._router.url);
      })
      this.setActiveTab(this._router.url);
   
    
  }

  setActiveTab(url:string){
    this.selectedTab = Object.keys(this.routeToTabMap).find(key => url.includes(key)) 
    ? this.routeToTabMap[Object.keys(this.routeToTabMap).find(key => url.includes(key))!] : 0
   
  }

  navigateToUserProfile(){
   
    this._router.navigate([`/Profile/${this.username}`])
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
  private _initilize(){
    this.username = this._userSessionService.getFromCookie("username")
    this.cdr.detectChanges()
  }
}
