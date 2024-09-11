import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { catchError, finalize, tap, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
  providers:[DatePipe]
})
export class PostComponent implements OnInit{
  @Input() post:any
  formattedDate:string = ""
  username:string = ""
  isLiked:boolean | null = null
  constructor(
    private _datePipe:DatePipe,
    private _postService:PostService,
    private _userSession:UserSessionService
  ){
   
  }

  ngOnInit():void{
    this.initilizeComponent();
  }

  initilizeComponent(){
    this.username = this._userSession.getUsername();
    this.formatDate()
    this.checkIsLiked()
  }
  checkIsLiked(){
    const foundUser = this.post.likedByUsers.find((user:any) => user.username === this.username);
    if(foundUser) {
      this.isLiked = true
    }else this.isLiked = false
  }
  formatDate(){
    const date = this.post.publishDate;
      const dateObject = new Date(date)

      if (isNaN(dateObject.getTime())) {
        this.formattedDate = 'Invalid Date';
      } else {
        this.formattedDate = this._datePipe.transform(dateObject, 'd MMM \'at\' HH:mm') || 'Invalid Format';
      }
  }

  likePost(postID:number){
    
    
    this._postService.likePost(postID, this.username).pipe(
      tap((response)=>{
        this.isLiked = !this.isLiked
        console.log("Successfully liked the post")
      }),
      catchError((error:HttpErrorResponse)=>{
        console.log(error)
        return throwError(error);
      }),
      finalize(()=>{
        console.log("End")
      })
    ).subscribe()
  }
}
