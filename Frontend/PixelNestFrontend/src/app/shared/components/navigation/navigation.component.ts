import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
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
  public username:string | null = this._userSessionService.getFromCookie("username")

  routeToTabMap: { [key: string]: number } = {
    'Dashboard/Feed': 1,
    ["Profile/"+this.username]: 4,
    'Messages': 2,
    'Notifications': 3,

  };
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _authService:AuthenticationService,
    private _router:Router,
    private cdr: ChangeDetectorRef
  ){

  }
  ngOnInit():void{
    this._initilize()
 
    this._router.events.pipe(filter(event=>event instanceof NavigationEnd))
    this.setActiveTab(this._router.url);
  }

  setActiveTab(url:string){
    this.selectedTab = Object.keys(this.routeToTabMap).find(key => url.includes(key)) 
    ? this.routeToTabMap[Object.keys(this.routeToTabMap).find(key => url.includes(key))!] 
    : 0;
   
  }

  navigateToUserProfile(){
    this._userSessionService.setToCookie("profileUsername", this.username);
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
    let email = this._userSessionService.getFromCookie("email");
    this._authService.logout(email).subscribe(response =>{
      console.log(response)
      this._router.navigate(["/Authentication/Login"])
      this._userSessionService.clearCookies();
      this._userSessionService.clearStorage();
    },(error:HttpErrorResponse)=>{
      console.log(error)
    })
  }
  private _initilize(){
    this.username = this._userSessionService.getFromCookie("username")
    this.selectedTab = this._userSessionService.getFromLocalStorage("tab") ?? 1;
    
    this.cdr.detectChanges()
  }
}
