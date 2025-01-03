import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, Subscription, tap, throwError } from 'rxjs';
import { LikedByUsers } from 'src/app/core/dto/likedByUsers.dto';
import { PostDto } from 'src/app/core/dto/post.dto';
import { SavedPosts } from 'src/app/core/dto/savePost.dto';
import { PostService } from 'src/app/core/services/post/post.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';
import { environment } from 'src/environments/environment.development';
import { LottieLoadingComponent } from '../lottie-loading/lottie-loading.component';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
  providers:[DatePipe]
})
export class PostComponent implements OnInit{


  @Input() post!:PostDto;
  likedByUsers: LikedByUsers[] = [];
  savedByUsers:SavedPosts[] = []

  totalComments:number = 0

  formattedDate:string = ""
  username:string = ""
  baseUrl:string = "";

  isLiked:boolean | null = null;
  isLikesTabOpen:boolean = false;
  areCommentsOpened:boolean = false;
  deleteDialog:boolean = false;
  isVisible:boolean = false;

  marginLeft = 0;
  progressBarMargin = 0;
  index = 0;
  subscription:Subscription = new Subscription;
  constructor(
    private _datePipe:DatePipe,
    private _postService:PostService,
    private _userSession:UserSessionService,
    private _userService:UserService,
    private _dashboardState:DashboardStateService,
    private _router:Router,
    private _postState:PostStateService,
    private _lottie:LottieStateService
  ){
   
  }

  ngOnInit():void{
    this.baseUrl = environment.blobStorageBaseUrl;
    this._initilizeComponent();
    
  }
  onSwipeRight() {
 
    if(this.index - 1 >= 0){
      this.marginLeft += 200;
      this.progressBarMargin = this.progressBarMargin - 100 / (this.post.imagePaths.length)
      this.index -= 1;
    }
  }
  onSwipeLeft() {
   
    if(this.index + 1 <= (this.post.imagePaths.length -1)){
      this.marginLeft -= 200;
      this.progressBarMargin = this.progressBarMargin + 100 / (this.post.imagePaths.length)
      this.index += 1;
    }
  }
  

  closeLikesTab() {
    this.isLikesTabOpen = false
  }

  closeCommentsTab(){
    this.areCommentsOpened = false;
  }
  closeDeleteDialog(){
    this.deleteDialog = false;
  }

  showLikes(){
    this.isLikesTabOpen = true
  }

  showComments(){
    this.areCommentsOpened = true;
    this._userSession.setToLocalStorage("postID",this.post.postID);
  }
  openDeleteDialog(){
    this.deleteDialog = true;
  }
  likePost(postID:number){
    this._postService.likePost(postID, this.username).pipe(
      tap((response)=>{
        this._handleLikeArray();
      
      }),
      catchError((error:HttpErrorResponse)=>{
        console.log(error)
        return throwError(error);
      })
    ).subscribe()
  }
  isSavedByUser(){
    return this.post.savedByUsers.find(a=>a.username == this.username);
  }
  savePost(){
    this._postService.savePost(this.username, this.post.postID).subscribe({
      next:response=>{
       
        this._handleSavedArray()
      }
    })
  }
  private _initilizeComponent(){
    this.username = this._userSession.getFromCookie("username");
    this.likedByUsers = this.post.likedByUsers;
    this.savedByUsers = this.post.savedByUsers;

   
    
    this._checkIsLiked()
  }

  private _handleSavedArray(){
    
    if(this.isSavedByUser()){
      this.post.savedByUsers = this.post.savedByUsers.filter(a => a.username != this.username);
    }else this.post.savedByUsers.push({username:this.username});
  }

  private _checkIsLiked(){
    const foundUser = this.post.likedByUsers.find((user:any) => user.username === this.username);
    this.likedByUsers = this.post.likedByUsers;
    if(foundUser) {
      this.isLiked = true
    }else this.isLiked = false
  }



  
  private _handleLikeArray(){
    if(!this.isLiked) this.likedByUsers.push({username: this.username})
    else this.likedByUsers = this.likedByUsers.filter(user => user.username !== this.username)
    this.isLiked = !this.isLiked

  }

}
