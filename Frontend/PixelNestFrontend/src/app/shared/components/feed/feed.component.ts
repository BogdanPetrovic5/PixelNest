import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
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
export class FeedComponent implements OnChanges, OnDestroy, OnInit{
  @Input() inputPosts:PostDto[] = [];
  posts:PostDto[] = []
  temporalResponse:PostDto[] = []
  currentPage:number = 1;
  isLoading:boolean = false;

  isEmpty:boolean = false;

  searchSubject: Subject<void> = new Subject<void>();
  constructor(
    private _postState: PostStateService
  ){}
  ngOnInit(): void {
      
      this.currentPage = 1;
  }
  ngOnDestroy(): void {
  
  }
  ngOnChanges(): void {
      this.posts = this.inputPosts;
  }
  loadMore(event:any){
    if(!this.isEmpty){
      const scrollElement = event.target;
      if ((scrollElement.offsetHeight + 1) + scrollElement.scrollTop >= scrollElement.scrollHeight) {
        this.currentPage += 1;
        this._postState.loadMore(this.currentPage);
       
      }
    }
   
  }

}
