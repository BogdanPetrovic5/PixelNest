import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent {
 @Input() comments:any;
 ngOnInit():void{
  console.log(this.comments)
 }
}
