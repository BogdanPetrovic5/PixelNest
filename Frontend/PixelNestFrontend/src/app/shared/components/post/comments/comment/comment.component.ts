import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent implements OnInit{
  @Input() comment:any;
  replies:any

  flattenReplies:any;

  repliesVisible:boolean = false;
  ngOnInit(): void {
    
    
  }
  haveReplies(){
    return this.comment.replies.length > 0
  }
  
  showReplies(){
   
    if (this.repliesVisible) {
     
      // this.replies = [];
      this.flattenReplies = []
    } else {
    
      this.flattenReplies = this._flattenReplies(this.comment)
      this.flattenReplies.shift()
    }
    this.repliesVisible = !this.repliesVisible;
  }


  private _flattenReplies(comment: any):any[]{
    let flattened = [];
    flattened.push(comment);

    if (comment.replies && comment.replies.length > 0) {
     
      for (const reply of comment.replies) {
        flattened = flattened.concat(this._flattenReplies(reply));
      }
    }

    return flattened;
  }
}
