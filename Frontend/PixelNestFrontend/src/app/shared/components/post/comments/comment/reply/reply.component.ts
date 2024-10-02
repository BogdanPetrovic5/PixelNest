import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { FlattenReplies } from 'src/app/core/dto/flattenReplies.dto';
import { Replies } from 'src/app/core/dto/replies.dto';
import { PostService } from 'src/app/core/services/post/post.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-reply',
  templateUrl: './reply.component.html',
  styleUrls: ['./reply.component.scss']
})
export class ReplyComponent {

  constructor(
    private _userSession:UserSessionService,
    private _postService:PostService,
    private _router:Router
  ){

  }
  @Input() reply!:FlattenReplies;
  @Input() commentID?:number;
  @Input() parentUsername:string = ""
  
  @Output() notifyParent: EventEmitter<{message:string}> = new EventEmitter();

  postID:number = 0;

  replyText:string = "";
  username:string = "";


  isReplyBox:boolean = false;

  navigateToProfile(username?:string){
    this._router.navigate([`Profile/${username}`])
  }

  trimAtSymbol(username: string): string {
    return username.replace(/^@/, ''); 
  }

  openReplyBox(){
    this.isReplyBox = !this.isReplyBox
  }
  replyToComment(){
    console.log(this.commentID);
    this.username = this._userSession.getFromCookie("username")
    this.postID = this._userSession.getFromLocalStorage("postID");
    
    this._postService.addComment(this.replyText, this.username, this.postID, this.commentID).subscribe({
      next:(response)=>{
        const message = {message:"fromChild"}
        this.notifyParent.emit(message);
      }
    })
    
  }
  
}
