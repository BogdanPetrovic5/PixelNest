import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { catchError, debounceTime, filter, switchMap, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { PostDto } from 'src/app/core/dto/post.dto';
import { Subject } from 'rxjs';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { CustomRouteReuseStrategy } from 'src/app/core/route-reuse-strategy';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnChanges, OnDestroy, OnInit{
  @Input() inputPosts:PostDto[] = [];
  
  posts:PostDto[] = []
  temporalResponse:PostDto[] = []
  currentPage:number = 1;
  isLoading:boolean = false;
  currentUrl:string =""
  isEmpty:boolean = false;
  scrollPosition:any
  searchSubject: Subject<void> = new Subject<void>();
  routerSubscription:any
  postsList:any

  constructor(
    private _postState: PostStateService,
    private _router: Router,
    private _cdr:ChangeDetectorRef,
    private _userSession:UserSessionService,
    private _feedState:DashboardStateService
  ){
   
  }
  ngOnInit(): void {

      this._userSession.deleteKeyFromCookie("scrlPos")
      this.currentUrl = this._router.url; 
      if (this.currentUrl === '/dashboard/feed') {
      
        this.currentPage = this._postState.feedCurrentPage;
      }else if(this.currentUrl.includes("profile/",0)) this.currentPage = this._postState.profileCurrentPage
      else if(this.currentUrl.includes('location/', 0)) this.currentPage = this._postState.locationCurrentPage
      this._userSession.currentUrl$.subscribe({
        next:response=>{
          if(response == "Feed") {
          
            this.restoreScrollPosition()
          }
        }
      })
     
  }
  ngOnDestroy(): void {
  
   
  }
  ngOnChanges(): void {
      this.posts = this.inputPosts;
    
  }
  ngDoCheck(): void{
   
  
  }

  restoreScrollPosition(): void {
    const postsList = document.querySelector('.posts-list') as HTMLElement;
    if (postsList && this.scrollPosition !== undefined) {
      postsList.scrollTop = this._feedState.getScrollPosition() ?? 0
    }
  }
  loadMore(event:any){
    const target = event.target as HTMLElement;
    if(this._router.url === '/dashboard/feed'){
      this.scrollPosition = target.scrollTop;
      this._feedState.setNewScrollPosition(this.scrollPosition)
    }
    
   
    if(!this.isEmpty){
      const scrollElement = event.target;
      if ((scrollElement.offsetHeight + 1) + scrollElement.scrollTop >= scrollElement.scrollHeight) {
        if (this.currentUrl === '/dashboard/feed') {
          this._postState.feedCurrentPage += 1;
        }else if(this.currentUrl.includes("profile/",0)){
          this._postState.profileCurrentPage += 1;
        }else if(this.currentUrl.includes('location',0)){
          this._postState.locationCurrentPage += 1;
        }
        
        this.currentPage += 1;
        this._postState.loadMore(this.currentPage);
        
      }
    }
   
  }

}
