import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { catchError, Subscription, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { PostDto } from 'src/app/dto/post.dto';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit{
  newPost:boolean | null = false;
  subscriptions: Subscription = new Subscription();

  post:PostDto[] = []
  constructor(
    private _dashboardStateMenagment:DashboardStateService, 
    private _postService: PostService){

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
    this.loadPosts()
  }

  loadPosts(){
    this._postService.getPosts()
    .pipe(
      catchError((error:HttpErrorResponse)=>{
        console.log(error)
        return throwError(()=>error);
      })
    ).subscribe(response =>{
      this.post = response
      console.log(this.post);
    })
  }
}
