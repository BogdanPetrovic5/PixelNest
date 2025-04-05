import { Component, OnInit,ChangeDetectorRef, OnDestroy, ViewChild, Input  } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { catchError, debounceTime, filter, of, Subject, Subscription, switchMap, takeUntil, tap } from 'rxjs';
import { PostDto } from 'src/app/core/dto/post.dto';
import { ProfileUser } from 'src/app/core/dto/profileUser.dto';
import { PostService } from 'src/app/core/services/post/post.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
import { ProfileStateService } from 'src/app/core/services/states/profile-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';
import { ProfileImageComponent } from 'src/app/shared/components/profile-image/profile-image.component';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy{
  @ViewChild(ProfileImageComponent) profilePicture!:ProfileImageComponent

  username:string = ""
  clientGuid:string = ""
  user:ProfileUser = {
    username: this.username,
    followers: 0,
    followings: 0,
    name: '',
    lastname: '',
    totalPosts: 0,
     clientGuid:'',
     canFollow:false,
     canEdit:false,
     chatID:'',
      email:'',
        profileImagePath:''
  }
  private destroy$ = new Subject<void>();
  posts:PostDto[] = []
  isLoading:boolean = false
  followersTab:boolean = false
  followingsTab:boolean = false;
  empty:boolean = false;
  isFollowing:boolean = false;
  editProfile:boolean = false;
  currentPage:number = 1;
  stringUrl!:string
  subscribe:Subscription = new Subscription;
 
  constructor(
    private _userSessions:UserSessionService,
    private _userService:UserService,
    private _postService:PostService,
    private _route:ActivatedRoute,
    private _router:Router,
    private _postState:PostStateService,
    private _cdr:ChangeDetectorRef,
    private _profileState:ProfileStateService,
    private _chatState:ChatStateService
  ){}
  ngOnInit(): void {
   
    this._userSessions.setUrl("Profile")
    this._postState.setPosts([]);
 
    this._initializeComponent();
    
  }

  ngOnDestroy(): void {
      this.subscribe.unsubscribe();
      this._postState.setPosts([]);
      this.user = {
        username: '',
        followers: 0,
        followings: 0,
        name: '',
        lastname: '',
        totalPosts: 0,
        clientGuid:'',
        canFollow:false,
        canEdit:false,
        chatID:'',
        email:'',
        profileImagePath:''
      }
      this._postState.setQuery(undefined);
      console.clear()
      
  }

  navigate(route:string, clientRoute:string, chatRoute:string){

    this._router.navigate([`/${route}/${clientRoute}/${chatRoute}`])
    this._chatState.setUser(this.user);
  }

  toggleEdit(){
    this.editProfile = !this.editProfile;
    if(this.editProfile == false) this._cdr.detectChanges();
  }

  follow(){
    this.isFollowing = !this.isFollowing
   
    if(this.user){
      this.subscribe.add(
        this._userService.follow(this.user.clientGuid).subscribe({
          next:response=>{
            if(!this.isFollowing) this.user.followers -= 1;
            else this.user.followers += 1;
            
          }
        })
      )
    }
    
  }

  checkIsFollowing(){
   if(this.user){
    this._userService.isFollowing(this.user.clientGuid).subscribe({
      next:response=>{
        this.isFollowing = response
     
      }
    })
   }
    
  }

  toggleFollowings(){
    this.followingsTab = !this.followingsTab
  }

  toggleFollowers(){
    this.followersTab = !this.followersTab;
  }


  private _loadData(){
    this.subscribe.add(
      this._userService.getUserData(this.clientGuid).pipe(
        tap(response => {
          this.user = response;
         
          this.checkIsFollowing();
          this._postState.setQuery(`clientGuid=${this.user.clientGuid}`);
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

  private _initializeComponent(){
    
    // this.subscribe.add(
    //   this._route.paramMap.subscribe(params => {
    //     this.username = params.get("username") || this._userSessions.getFromCookie("username")
    //     this._resetProfileState();
    //     this._loadData()
    //      this._userService.getProfilePicture(this.username).subscribe({next:response=>{
    //             if(response.path.length > 0){
    //               this.stringUrl = environment.blobStorageBaseUrl + response.path;
    //             }
    //      }}) 
    //   })

    // );


    // this._route.paramMap
    // .pipe(
    //   takeUntil(this.destroy$),
    //   switchMap(params => {
    //     this.username = params.get('username') ?? this._userSessions.getFromCookie('username');
        
    //     this._resetProfileState();
    //     this._loadData();
       
    //     return this._userService.getProfilePicture(this.username!);
    //   })
    // )
    // .subscribe({
    //   next: response => {
    //     this.followersTab = false;
    //     this.followingsTab = false;
    //     this.profilePicture.username = this.username;
    //     if (response.path?.length > 0) {
    //       this.stringUrl = `${environment.blobStorageBaseUrl}${response.path}`;
    //     }

    //   },
    //   error: err => {
    //     console.error('Failed to load profile picture:', err);
    //   }
    // });

    this._router.events
      .pipe(
  
        filter((event): event is NavigationStart => event instanceof NavigationStart)
      )
      .subscribe((event: NavigationStart) => {
       this.editProfile = false;
       this.followersTab = false;
       this.followingsTab = false;
       console.clear()
       
    });

    this._route.paramMap
    .pipe(
      takeUntil(this.destroy$), 
      tap(params => {
       
        this.clientGuid = params.get('username') ?? this._userSessions.getFromCookie('userID');
       
        this._resetProfileState();
        this._loadData();
      })
    )
    .subscribe({
      error: err => {
        console.error('Error during profile setup:', err);
      }
    });
    this.subscribe.add(
      this._profileState.currentProfileUrl$.subscribe({
        next:response =>{
         
          this.stringUrl = response
        }
      })
    )
  }
  private _resetProfileState() {
  
    this._postState.setPosts([]);
   
    this.posts = [];
    this.currentPage = 1;
    this.empty = false;
    this.isLoading = false;
    
  }
}
