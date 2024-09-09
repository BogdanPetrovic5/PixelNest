import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { PostDto } from 'src/app/dto/post.dto';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit{
  posts:PostDto[] = []
  constructor(private _postService: PostService){

  }
  ngOnInit(): void {
      this.initilizeApp();
  }
  initilizeApp(){
    this.loadPosts()
  }
  loadPosts(){
    this._postService.getPosts()
    .pipe(
      catchError((error:HttpErrorResponse)=>{
        console.log(error)
        return throwError(()=>error);
      })
    ).subscribe(response =>{
      this.posts = response
      console.log(this.posts);
    })
  }
}
