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
  constructor(
    private _datePipe:DatePipe,
    private _postService:PostService,
    private _userSession:UserSessionService
  ){
   
  }

  ngOnInit():void{
    const date = this.post.publishDate;
    const dateObject = new Date(date)

    if (isNaN(dateObject.getTime())) {
      this.formattedDate = 'Invalid Date';
    } else {
      this.formattedDate = this._datePipe.transform(dateObject, 'd MMM \'at\' HH:mm') || 'Invalid Format';
    }
  }

  likePost(postID:number){
    let username = this._userSession.getUsername();
    
    this._postService.likePost(postID, username).pipe(
      tap((response)=>{
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
