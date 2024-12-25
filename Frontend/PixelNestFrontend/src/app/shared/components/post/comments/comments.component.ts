import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { catchError, switchMap, tap, throwError } from 'rxjs';
import { CommentDto } from 'src/app/core/dto/comment.dto';
import { CommentService } from 'src/app/core/services/comment/comment.service';
import { PostService } from 'src/app/core/services/post/post.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent {
  comments: CommentDto[] = [];
  @Output() closeCommentsTab: EventEmitter<void> = new EventEmitter<void>();
  username:string = ""

  commentText:string = ""
  postID:number = 0;
  constructor(
    private _commentService:CommentService,
    private _postService:PostService,
    private _userSession:UserSessionService
  ){

  }
  ngOnInit():void{
     this._initilizeApp();
  }
  
  addComment(inputElement: HTMLInputElement){
    this._postService.addComment(this.commentText, this.username, this.postID).pipe(
      tap((response)=>{
       

        this.comments.push({username:this.username, commentText:this.commentText})
        this.commentText = ""
        inputElement.blur();
      }),
      switchMap(()=>this.getComments()),
      catchError((error: HttpErrorResponse) => {
        console.error('Error adding comment:', error);
        return throwError(() => error);
      })
    ).subscribe({
      next:(newComments)=>{
        this.comments = newComments;
      }
    })
  }
  
  getComments(){

   return this._commentService.getComments(this.postID).pipe(

      tap((response)=>{
        console.log('Fetched comments:', response)
       
      }),
      catchError((error: HttpErrorResponse) => {
        console.error('Error fetching comments:', error);
        return throwError(() => error);
      })
    )
  }
  private _initilizeApp(){
    this.postID = this._userSession.getFromLocalStorage("postID");
    this.username = this._userSession.getFromCookie("username")
    this.getComments().subscribe({
      next: (comments) => {
        this.comments = comments; 
      },
      error: (error) => {
        console.error('Error fetching comments:', error);
      }
    });
  }
  close(){
    this.closeCommentsTab.emit()
  }
}
