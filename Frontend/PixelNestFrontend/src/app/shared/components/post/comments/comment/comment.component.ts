import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, tap, throwError } from 'rxjs';
import { CommentDto } from 'src/app/core/dto/comment.dto';
import { FlattenReplies } from 'src/app/core/dto/flattenReplies.dto';
import { Replies } from 'src/app/core/dto/replies.dto';
import { CommentService } from 'src/app/core/services/comment/comment.service';
import { PostService } from 'src/app/core/services/post/post.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent implements OnInit{

  constructor(
    private _userSession:UserSessionService,
    private _postService:PostService,
    private _commentService:CommentService,
    private _router:Router
  ){

  }
  @Input() comment!:CommentDto;


  flattenReplies:FlattenReplies[] = [];
  replies:Replies[] =[];
  username:string = "";
  replyText:string = "";

  postID:string = '';


  repliesVisible:boolean = false;
  isReplyBox:boolean = false;

  isDisabled:boolean = false;
  ngOnInit(): void {
    this.username = this._userSession.getFromCookie("username")
    this.postID = this._userSession.getFromLocalStorage("postID");
    
  }
  
  replyComment(parentCommentID?:number){
    
   

    this._postService.addComment(this.replyText, this.username, this.postID, parentCommentID).subscribe({
      next:(response)=>{

        this.replyText = ""
        this.isDisabled = false;
        this.getReplies()
        
      }
    })
    
  }

  haveReplies(){
    if(this.comment.totalReplies != undefined){
      return this.comment.totalReplies > 0
    }
    return false
    
  }

  toggleReplies(){
    if (this.repliesVisible) {
      this.flattenReplies = []
    } else {
      this.flattenReplies = this._flattenReplies(this.replies)
     
    }
    this.repliesVisible = !this.repliesVisible
  }

  getReplies(data?: { message: string}){

    if(data?.message === "fromChild"){
      this.isDisabled = false;
    }

    if(this.isDisabled == false){
      this._commentService.getReplies(this.comment.commentID).pipe(
        tap((response)=>{
          this.replies = response
          this.isDisabled = true;
        }),
        map(
          (response)=>this._flattenReplies(response)
        ),
        catchError((error: HttpErrorResponse) => {
          console.log(error);
          return throwError(() => error);
        })

      ).subscribe((response)=>{
        this.flattenReplies = response;
      })
    }
   
    
  }

  openParentReply(){
    this.isReplyBox = !this.isReplyBox
  }

  openReplyBox(i:number){
   this.flattenReplies[i].isReplyBox = !this.flattenReplies[i].isReplyBox
  }

  findUsername(parentCommentID?:number | null){
    for(let i = 0; i < this.flattenReplies.length;i++){
      if(this.flattenReplies[i].commentID == parentCommentID){
        if(this.flattenReplies[i].parentCommentID == null){
          return ""
        }
      }
    }

    let username;
    for(let i = 0; i < this.flattenReplies.length;i++){
      if(this.flattenReplies[i].commentID == parentCommentID){
        if(this.flattenReplies[i].username != undefined){
          username = this.flattenReplies[i].username;
        }
       
        return "@" + username;
      }
    }
    return ""
  }

  private _flattenReplies(comment: any):any[]{
    let flattened = [];
    for (const reply of comment) {
      const { replies, ...rest } = reply;
      flattened.push({
          ...rest
      });

      if (reply.replies && reply.replies.length > 0) {
          flattened = flattened.concat(this._flattenReplies(reply.replies));
      }
    }

    return flattened;
  }
}
