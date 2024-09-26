import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit{
  public selectedTab:number | null = 1;
  public username:string | null = this._userSessionService.getFromCookie("username")
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _authService:AuthenticationService,
    private _router:Router,
    private cdr: ChangeDetectorRef
  ){

  }
  ngOnInit():void{
   
    this.initilize()
    console.log(this.username)
  }
  navigate(route:string){
    this._router.navigate([route])
  }
  initilize(){
    this.username = this._userSessionService.getFromCookie("username")
    this.cdr.detectChanges()
  }
  openNewPostDialog(){
    this._dashboardStateMenagment.setIsTabSelected(true);
  }

  changeTab(tabIndex:number){
    this.selectedTab = tabIndex
    console.log('Tab changed, current username:', this.username);
  }
  logout(){
    let email = this._userSessionService.getFromCookie("email");
    this._authService.logout(email).subscribe(response =>{
      console.log(response)
      this._router.navigate(["/Authentication/Login"])
      this._userSessionService.clearCookies();
    },(error:HttpErrorResponse)=>{
      console.log(error)
    })
  }
}
