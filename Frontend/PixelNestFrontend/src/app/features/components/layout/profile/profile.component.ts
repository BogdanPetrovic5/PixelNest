import { Component, OnInit,ChangeDetectorRef, OnDestroy  } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { filter, Subscription, switchMap } from 'rxjs';
import { PostDto } from 'src/app/core/dto/post.dto';
import { ProfileUser } from 'src/app/core/dto/profileUser.dto';
import { PostService } from 'src/app/core/services/post/post.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy{
  username:string = ""
  user!:ProfileUser

  posts:PostDto[] = []
  isLoading:boolean = false
  followersTab:boolean = false
  followingsTab:boolean = false;
  empty:boolean = false;
  
  
  currentPage:number = 1;

  subscribe:Subscription = new Subscription;
 
  constructor(
    private _userSessions:UserSessionService,
    private _userService:UserService,
    private _postService:PostService,
    private _cdr:ChangeDetectorRef,
    private _router:Router,
    private _route:ActivatedRoute
  ){}
  ngOnInit(): void {
    this._initilizeApp()
  
    this.subscribe.add(
      this._router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
  
      )
      .subscribe((response:any) => {
        window.location.reload()
      })
     
    )
  
  }

  ngOnDestroy(): void {
      this.subscribe.unsubscribe();
  }

  isFollowing(){
    let username = this._userSessions.getFromCookie("username");
    this._userService.isFollowing(username, this.user.username).subscribe({
      next:response=>{
        return response;
      }
    })
  }

  toggleFollowings(){
    this.followingsTab = !this.followingsTab
  }

  toggleFollowers(){
    this.followersTab = !this.followersTab;
  }

  loadMore(event:any){
    if(!this.empty){
      const scrollElement = event.target;
      if ((scrollElement.offsetHeight) + scrollElement.scrollTop >= scrollElement.scrollHeight) {
        this.currentPage += 1;
        this._loadPosts()
      }
    }
   
  }

  checkCurrentUser(){
    return this._userSessions.getFromCookie("username") == this.username
  }

  private _loadData(){
    this.subscribe.add(
      this._userService.getUserData(this.username).subscribe({
        next:response=>{
          this.user = response
          this._loadPosts();
        },
        error: error => {
          console.error('An error occurred:', error);
        }
      })
    )
   
    
  }

  private _loadPosts(){
    
    this.isLoading = true;
    this.subscribe.add(
      this._postService.getPostsByUsername(this.username, this.currentPage).subscribe({
        next:response=>{
       
          if(response.length < 5) this.empty = true;
          console.log(this.currentPage);
          this.posts = this.posts.concat(response);
          this.isLoading = false;
        },
        error:error=>{
          console.log(error);
        }
      })
    )
   
  }
  private _initilizeApp(){
   this._resetProfileState();
    this.subscribe.add(
      this._route.paramMap.subscribe(params => {
        this.username = params.get("username") || this._userSessions.getFromCookie("username")
        
        this._loadData()
      })

    );
   
    
  }
  private _resetProfileState() {
   
    this.posts = [];
    this.currentPage = 1;
    this.empty = false;
    this.isLoading = false;
    console.log("RESET", this.currentPage)
  }
}
