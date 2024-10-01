import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { PostDto } from 'src/app/core/dto/post.dto';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit{
  posts:PostDto[] = []
  temporalResponse:PostDto[] = []
  currentPage:number = 1;
  isLoading:boolean = false;
  constructor(private _postService: PostService){

  }
  ngOnInit(): void {
      this._initilizeApp();
  }
  
  loadPosts(){
    this.isLoading = true;
    this._postService.getPosts(this.currentPage)
    .pipe(
      catchError((error:HttpErrorResponse)=>{
        console.log(error)
        return throwError(()=>error);
      })
    ).subscribe(response =>{
      this.temporalResponse = response
      this.posts = this.posts.concat(this.temporalResponse);
      this.isLoading = false;
     
    })
  }

  loadMore(event:any){
    const scrollElement = event.target;
    if ((scrollElement.offsetHeight + 50) + scrollElement.scrollTop >= scrollElement.scrollHeight) {
      this.currentPage += 1;
      this.loadPosts()
    }
  }
 private _initilizeApp(){
    this.loadPosts()
  }
}
