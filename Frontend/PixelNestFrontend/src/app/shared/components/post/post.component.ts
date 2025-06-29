import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
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
import Hammer from 'hammerjs';
@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
  providers:[DatePipe],

})
export class PostComponent implements OnInit{


  @Input() post!:PostDto;
  @Input() postIndex!:number
  @Input() postID!:string

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
  isDragging = false;
  containerWidth = 0;
  
  marginLeft:number = 0;
  currentMargin:number = 0;
  startX:number = 0;

  progressBarMargin:number = 0;
  index:number = 0;
  
  subscription:Subscription = new Subscription;
  constructor(

    private _postService:PostService,
    private _userSession:UserSessionService,
    private _postState:PostStateService
  ){
   
  }
  ngAfterViewInit() {
  
    this.containerWidth = this.getImageWidth();
 
  }
  ngOnInit():void{
    this.baseUrl = environment.blobStorageBaseUrl;
    this._initializeComponent();
    
    const element = document.getElementById(`image-slider-${this.postIndex}`); 
    if(element){
      const hammer = new Hammer(element);
      hammer.get('pan').set({
        threshold: 3,   
        velocity: 0.5    
      });

    }
   
  }
  

  onPanStart(event:any, index:number){
  
  
    this.isDragging = true;
    this.startX = event.touches[0].clientX;
    this.currentMargin = this.marginLeft
  }
  onPanMove(event:any,index:number){
  
    if (!this.isDragging) return;

    const delta = (event.touches[0].clientX - this.startX) *  1.3;
    const potentialMargin = this.currentMargin + delta;
  

    const maxMargin = 0;
    const minMargin = -(this.post.imagePaths.length - 1) * this.containerWidth;
  
    this.marginLeft = Math.max(minMargin, Math.min(maxMargin, potentialMargin));
     
    this.updateSliderTransform(false);
  }
  onPanEnd(event: any,index:number) {
   
      this.isDragging = false;
      const threshold = this.containerWidth * 0.25; 
      const delta = event.changedTouches[0].clientX - this.startX;
      
      if (delta < -threshold && this.index < this.post.imagePaths.length - 1) {
        this.moveProggressBar(delta)
        this.index++;
       
      } else if (delta > threshold && this.index > 0) {
        this.moveProggressBar(delta)
        this.index--;
      }
      this.marginLeft = -this.index * this.containerWidth;
 
      this.updateSliderTransform(true);
  }
  updateSliderTransform(smooth: boolean) {
    const slider = document.getElementById(`image-slider-${this.postIndex}`);
    if (slider) {
     
      slider.style.transition = smooth ? 'transform 0.2s ease-out' : 'none';
      slider.style.transform = `translateX(${this.marginLeft}px)`;
    }
  }
  moveProggressBar(delta:number){
    if(delta > 0 && this.index - 1 >= 0){
      this.progressBarMargin = this.progressBarMargin - 100 / (this.post.imagePaths.length)
     }else if(this.index + 1 <= (this.post.imagePaths.length - 1) && delta < 0) this.progressBarMargin = this.progressBarMargin + 100 / (this.post.imagePaths.length)
  }
  getImageWidth(): number {
    const imageWrapper = document.querySelector('.image-wrapper') as HTMLElement;
    return imageWrapper ? imageWrapper.offsetWidth : 0;
  }
  onSwipeLeft() {
   
    if(this.index + 1 <= (this.post.imagePaths.length -1)){
      this.marginLeft -= this.containerWidth;
      this.progressBarMargin = this.progressBarMargin + 100 / (this.post.imagePaths.length)
      this.index += 1;
    }
  }

  onSwipeRight() {
 
    if(this.index - 1 >= 0){
      this.marginLeft += this.containerWidth;
      this.progressBarMargin = this.progressBarMargin - 100 / (this.post.imagePaths.length)
      this.index -= 1;
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
    this._userSession.setToLocalStorage("postID", this.post.postID);
  }
  openDeleteDialog(){
    this.deleteDialog = true;
  }

  likePost(postID:string){
    this._postService.likePost(postID).pipe(
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
    this._postService.savePost(this.post.postID).subscribe({
      next:response=>{
       
        this._handleSavedArray()
      }
    })
  }
  
  private _initializeComponent(){
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
    if(!this.isLiked) this.likedByUsers.push({username: this.username, clientGuid:this._userSession.getFromCookie("sessionID")})
    else this.likedByUsers = this.likedByUsers.filter(user => user.username !== this.username)
    this.isLiked = !this.isLiked


  }

}
