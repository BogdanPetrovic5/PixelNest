import { ChangeDetectorRef, Component, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { PostDto } from 'src/app/core/dto/post.dto';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
import { IndexedDbService } from 'src/app/core/services/indexed-db/indexed-db.service';
import { CacheService } from 'src/app/core/services/cache/cache.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { Router } from '@angular/router';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';


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
  isChanged:boolean = false
  
  subscriptions: Subscription = new Subscription();

  posts:PostDto[] = []
  searchSubject: Subject<void> = new Subject<void>();
  constructor(
    private _postState:PostStateService,
    private _indexedDb:IndexedDbService,
    private _cdr:ChangeDetectorRef,
    private _cacheService:CacheService,
    private _userSession:UserSessionService,
    private _router:Router,
    private _dashboardState:DashboardStateService
  ){


  }
  
  ngOnDestroy(): void {
  
    this.subscriptions.unsubscribe();
    this.posts = []


    this._postState.setPosts(this.posts);
  
  }
  ngOnChanges(): void{

  }
  ngOnInit(): void {
   
    this._cacheService.checkCache().subscribe({
      next:response=>{
 
        if(response){
        
          this.isChanged = response;
        }
        
        this._initializeComponent();
       
      }
    })
 
  }
  ngDoCheck(){
    if(this._router.url == "/Dashboard/Feed") this._userSession.setUrl("Feed")
  }
  loadPosts(){
    
    this._postState.loadPosts(this.currentPage);
    
  }


 private _initializeComponent(){
   

   
    if(this._postState.feedPosts.length > 0 && !this.isChanged) this.posts = this._postState.feedPosts;
    else{
      this._postState.resetFeed([])
      this.loadPosts()
      this._dashboardState.setIsPostApiFinished(true)
    }
    
    this.subscriptions.add(
      this._postState.isLoading$.subscribe({
        next:response=>{
          this.isLoading = response;
        }
      })
    )
    this.subscriptions.add(
      this._postState.feedPosts$.subscribe({
        next:response=>{
          this.posts = response;
          
        
        this._cdr.detectChanges()
        },error:error=>{
          console.error(error.message);
        },
        
      })
    )
    
  }

}
