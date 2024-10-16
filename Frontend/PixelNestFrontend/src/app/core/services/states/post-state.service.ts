import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, debounceTime, map, Observable, Subject, switchMap, tap, throwError } from 'rxjs';
import { PostDto } from '../../dto/post.dto';
import { PostService } from '../post/post.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PostStateService{
  private _postsSubject = new BehaviorSubject<PostDto[]>([]);
  private _loadingSubject = new BehaviorSubject<boolean>(false);
  
  public posts$ = this._postsSubject.asObservable();
  public isLoading$ = this._loadingSubject.asObservable();

  public posts:PostDto[] = [];
  private currentPage = 1;


  constructor(private _postService:PostService) { }

  setLoading(value:boolean){
    this._loadingSubject.next(value);
  }
  setPosts(value:PostDto[]){
    this._postsSubject.next(value);
    this.posts = value;
  }
  loadMore(currentPage:number){
    
    this.loadPosts(currentPage);
  }

  loadPosts(currentPage:number) {
    this.setLoading(true);
    this._postService.getPosts(currentPage).subscribe({
      next:response=>{
        this.posts = this.posts.concat(response);
        this._postsSubject.next(this.posts);
        this.setLoading(false)
      }
    })
  }
  
}
