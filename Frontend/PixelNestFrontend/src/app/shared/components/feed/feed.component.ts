import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { catchError, debounceTime, switchMap, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { PostDto } from 'src/app/core/dto/post.dto';
import { Subject } from 'rxjs';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnChanges{
  @Input() inputPosts:PostDto[] = [];
  posts:PostDto[] = []
  temporalResponse:PostDto[] = []
  currentPage:number = 2;
  isLoading:boolean = false;

  isEmpty:boolean = false;

  searchSubject: Subject<void> = new Subject<void>();
  constructor(
    private _postState: PostStateService
  ){}
  ngOnChanges(): void {
      this.posts = this.inputPosts;
  }
  loadMore(event:any){
    if(!this.isEmpty){
      const scrollElement = event.target;
      if ((scrollElement.offsetHeight + 1) + scrollElement.scrollTop >= scrollElement.scrollHeight) {
        this._postState.loadMore();
      }
    }
   
  }

}
