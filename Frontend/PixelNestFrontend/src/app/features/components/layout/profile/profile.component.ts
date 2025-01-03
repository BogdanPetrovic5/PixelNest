import { Component, OnInit,ChangeDetectorRef, OnDestroy  } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { catchError, debounceTime, filter, of, Subscription, switchMap, tap } from 'rxjs';
import { PostDto } from 'src/app/core/dto/post.dto';
import { ProfileUser } from 'src/app/core/dto/profileUser.dto';
import { PostService } from 'src/app/core/services/post/post.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
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
  isFollowing:boolean = false;
  editProfile:boolean = false;
  currentPage:number = 1;

  subscribe:Subscription = new Subscription;
 
  constructor(
    private _userSessions:UserSessionService,
    private _userService:UserService,
    private _postService:PostService,
    private _route:ActivatedRoute,
    private _postState:PostStateService,
    private _cdr:ChangeDetectorRef
  ){}
  ngOnInit(): void {
    this._postState.setPosts([]);
    this._initilizeApp();
  }

  ngOnDestroy(): void {
      this.subscribe.unsubscribe();
      this._postState.setPosts([]);
      this._postState.setQuery(undefined);
  }
  toggleEdit(){
    this.editProfile = !this.editProfile;
    if(this.editProfile == false) this._cdr.detectChanges();
  }
  follow(){
    this.isFollowing = !this.isFollowing
    let currentUsername = this._userSessions.getFromCookie("username")
    this.subscribe.add(
      this._userService.follow(currentUsername, this.username).subscribe({
        next:response=>{
          if(!this.isFollowing) this.user.followers -= 1;
          else this.user.followers += 1;
          
        }
      })
    )
   
  }

  checkIsFollowing(){
    let username = this._userSessions.getFromCookie("username");
    this._userService.isFollowing(username, this.user.username).subscribe({
      next:response=>{
        this.isFollowing = response
      }
    })
  }

  toggleFollowings(){
    this.followingsTab = !this.followingsTab
  }

  toggleFollowers(){
    this.followersTab = !this.followersTab;
  }

  checkCurrentUser(){
    return this._userSessions.getFromCookie("username") == this.username
  }

  private _loadData(){
    this.subscribe.add(
      this._userService.getUserData(this.username).pipe(
        tap(response => {
          this.user = response;
          this.checkIsFollowing();
          this._postState.setQuery(`username=${this.user.username}`);
          this._postState.loadPosts(1);
        }), 
        switchMap(() => this._postState.posts$), 
        tap(posts => {
          this.posts = posts;
          
        }),
        catchError(err => {
          console.error('Error:', err);
          return of([]); 
        })
      ).subscribe()
    );
  
  }

  private _initilizeApp(){
    
    this.subscribe.add(
      this._route.paramMap.subscribe(params => {
        this.username = params.get("username") || this._userSessions.getFromCookie("username")
        this._resetProfileState();
        this._loadData()
        
      })

    );
   
    
  }
  private _resetProfileState() {
    this._postState.setPosts([]);
    this.posts = [];
    this.currentPage = 1;
    this.empty = false;
    this.isLoading = false;
    
  }
}
