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
  private _queryParameterSubject = new BehaviorSubject<string | undefined>(undefined)
  
  private cache = new Map<string, { posts: PostDto[], timestamp: number }>();
  private cacheDuration = 1000 * 60 * 5;
  private cacheKey = "";
  public posts$ = this._postsSubject.asObservable();
  public isLoading$ = this._loadingSubject.asObservable();

  public posts:PostDto[] = [];
  
  public queryParameter?:string;
  public currentPage:number = 1;

  constructor(private _postService:PostService) { }

  setLoading(value:boolean){
    this._loadingSubject.next(value);
  }



  setPosts(value:PostDto[]){
    this._postsSubject.next(value);
    
  
    
    this.posts = value;
  }

  clearCache(){
    this.cache.delete(this.cacheKey);
    this.cacheKey = `${this.currentPage}-${this._queryParameterSubject.getValue()}`
    this.cache.set(this.cacheKey, {
      posts: [],
      timestamp: Date.now()
    });
  }
  setQuery(value?:string){
    this._queryParameterSubject.next(value)
  }

  loadMore(currentPage:number){
    this.loadPosts(currentPage);
  }

  loadPosts(currentPage:number) {
    const currentQuery = this._queryParameterSubject.getValue();
   
    this.setLoading(true);
    this._postService.getPosts(currentPage, currentQuery).subscribe({
      next:response=>{
        this.posts = this.posts.concat(response);
        this.cache.set(this.cacheKey, {
          posts: this.posts,
          timestamp: Date.now()
        });
        this._postsSubject.next(this.posts);
        this.setLoading(false)
      }
    })
  }
  
}
