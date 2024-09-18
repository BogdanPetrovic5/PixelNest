import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PostService } from 'src/app/core/services/post/post.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent {
  @Input() comments:{username:string, commentText:string}[] = [];
  @Output() closeCommentsTab: EventEmitter<void> = new EventEmitter<void>();
  username:string = ""

  commentText:string = ""
  postID:number = 0;
  constructor(
    
    private _postService:PostService,
    private _userSession:UserSessionService
  ){

  }
  ngOnInit():void{
    console.log(this.comments);
    
  }
  addComment(){
    this.username = this._userSession.getFromCookie("username")
    this.postID = this._userSession.getFromLocalStorage("postID");
    this._postService.addComment(this.commentText, this.username, this.postID).subscribe({
      next:(response)=>{
        console.log(response.message)
        this.comments.push({username:this.username, commentText:this.commentText})
      }
    })
  }
  close(){
    this.closeCommentsTab.emit()
  }
}
