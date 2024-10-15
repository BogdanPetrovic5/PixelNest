import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { catchError, debounceTime, Subject, Subscription, switchMap, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { PostDto } from 'src/app/core/dto/post.dto';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy, OnChanges{
  newPost:boolean | null = false;
  isLoading:boolean = false;
  currentPage:number = 1;
  isEmpty:boolean = false;

  
  subscriptions: Subscription = new Subscription();

  posts:PostDto[] = []
  searchSubject: Subject<void> = new Subject<void>();
  constructor(
    private _postState:PostStateService
  ){

  }
  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
  ngOnChanges(): void{

  }
  ngOnInit(): void {
    this.loadPosts();
  }

   loadPosts(){
    this.isLoading = true;
    this._postState.loadPosts();
    this.subscriptions.add(
      this._postState.posts$.subscribe({
        next:response=>{
          this.posts = response;
          console.log(this.posts);
          this.isLoading = false;
        },error:error=>{
          console.error(error.message);
        },
        
      })
    )
 
  
    
  }
  


}
