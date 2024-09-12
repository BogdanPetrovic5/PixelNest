import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
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
  public username:string | null = null
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _authService:AuthenticationService,
    private _router:Router
  ){

  }
  ngOnInit():void{
    this.initilize()
  }
  initilize(){
    this.username = this._userSessionService.getUsername()
  }
  openNewPostDialog(){
    this._dashboardStateMenagment.setIsTabSelected(true);
  }

  changeTab(tabIndex:number){
    this.selectedTab = tabIndex
  }
  logout(){
    let email = this._userSessionService.getEmail();
    this._authService.logout(email).subscribe(response =>{
      console.log(response)
      this._router.navigate(["/Authentication/Login"])
      this._userSessionService.clearCookies();
    },(error:HttpErrorResponse)=>{
      console.log(error)
    })
  }
}
