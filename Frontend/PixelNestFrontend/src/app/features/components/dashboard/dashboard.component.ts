import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { catchError, Subscription, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { PostDto } from 'src/app/core/dto/post.dto';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy{
  newPost:boolean | null = false;
  subscriptions: Subscription = new Subscription();

  post:PostDto[] = []
  constructor(
    private _dashboardStateMenagment:DashboardStateService, 
    private _postService: PostService,
    private _userSession: UserSessionService
  ){

  }
  ngOnDestroy(): void {
    // this._userSession.clearCookies()
  }
  ngOnInit(): void {
 
      this.initilizeApp()
  }

  initilizeApp(){
    this.subscriptions.add(
      this._dashboardStateMenagment.newPostTab$.subscribe(response =>{
        if(this.newPost != null && response != null){
          this.newPost = response
        }
      })
    )
    
  }


}
