import { Component, Input, OnInit } from '@angular/core';
import { LikedByUsers } from 'src/app/core/dto/likedByUsers.dto';
import { CommentService } from 'src/app/core/services/comment/comment.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-like-comment',
  templateUrl: './like-comment.component.html',
  styleUrls: ['./like-comment.component.scss']
})
export class LikeCommentComponent implements OnInit{
  constructor(
    private _userSession:UserSessionService, 
    private _commentService:CommentService
  ){

  }
  @Input() likedByUsers?:LikedByUsers[]
  @Input() commentUsername?:string = ""
  @Input() commentID?:number;

  currentUsername:string = ""
  
  
  ngOnInit():void{
    this.currentUsername = this._userSession.getFromCookie("username")
 
  }
  isLikedByUser(){
    
    return this.likedByUsers?.find(a => a.username === this.currentUsername)
  }
  likeComment(){
    this._commentService.likeComment(this.commentID, this.currentUsername).subscribe({
      next:(response)=>{
      
        this._handleLikedByUsersArray()
      }
      
    })
  }

  private _handleLikedByUsersArray(){
    if(this.isLikedByUser()){
      this.likedByUsers = this.likedByUsers?.filter(user => user.username !== this.currentUsername)
      return;
    }
    const object = { username:this.currentUsername, clientGuid:this._userSession.getFromCookie("userID") }
    this.likedByUsers?.push(object);
    return;
  }
}
