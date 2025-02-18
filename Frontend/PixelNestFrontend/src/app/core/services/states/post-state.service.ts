import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, debounceTime, map, Observable, Subject, switchMap, tap, throwError } from 'rxjs';
import { PostDto } from '../../dto/post.dto';
import { PostService } from '../post/post.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PostStateService{
  private _postsSubject = new BehaviorSubject<PostDto[]>([]);
  private _feedPostsSubject = new BehaviorSubject<PostDto[]>([]);
  private _loadingSubject = new BehaviorSubject<boolean>(false);
  private _queryParameterSubject = new BehaviorSubject<string | undefined>(undefined)
  private _locationPosts = new BehaviorSubject<PostDto[]>([])

  private cache = new Map<string, { posts: PostDto[], timestamp: number }>();
  private cacheDuration = 1000 * 60 * 5;
  private cacheKey = "";
  public posts$ = this._postsSubject.asObservable();
  public feedPosts$ = this._feedPostsSubject.asObservable();
  public locationPosts$ = this._locationPosts.asObservable()
  public isLoading$ = this._loadingSubject.asObservable();


  public posts:PostDto[] = [];
  public feedPosts:PostDto[] = []
  public locationPosts:PostDto[] = []

  public feedCurrentPage:number = 1;
  public profileCurrentPage:number = 1;
  public locationCurrentPage:number = 1;

  public queryParameter?:string;
  
  
  constructor(
    private _postService:PostService,
    private _activeRoute:ActivatedRoute
  ) { }

  setLoading(value:boolean){
    this._loadingSubject.next(value);
  }


  getQuery(){
    return this._queryParameterSubject.getValue()
  }
  resetFeed(value:PostDto[]){
    this._feedPostsSubject.next(value)
    this.feedPosts = value
  }
  setPosts(value:PostDto[]){
    this._postsSubject.next(value);
    this.posts = value;
  }
  resetLocationPosts(value:PostDto[]){
    this._locationPosts.next(value)
    
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
        console.log(response)
        this.posts = this.posts.concat(response);
        this.cache.set(this.cacheKey, {
          posts: this.posts,
          timestamp: Date.now()
        });
        
        if(currentQuery == undefined){
          this.feedPosts = this.feedPosts.concat(response)
          this._feedPostsSubject.next(this.feedPosts)
          
          this.setLoading(false)
          return
        }
        
        this._postsSubject.next(this.posts);
        this.setLoading(false)
      }
    })
  }
  
}
