import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, debounceTime, map, Observable, Subject, switchMap, tap, throwError } from 'rxjs';
import { PostDto } from '../../dto/post.dto';
import { PostService } from '../post/post.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PostStateService implements OnInit{
  private postsSubject = new BehaviorSubject<PostDto[]>([]);
  public posts$ = this.postsSubject.asObservable();
  
  public posts:PostDto[] = [];
  private currentPage = 1;


  constructor(private _postService:PostService) { }

  ngOnInit():void{
    this.loadPosts();
  }
  
  loadMore(){
    this.currentPage += 1;
    this.loadPosts();
  }

  loadPosts() {
    this._postService.getPosts(this.currentPage).subscribe({
      next:response=>{
        this.posts = this.posts.concat(response);
        this.postsSubject.next(this.posts);
      }
    })
  }
  
}
