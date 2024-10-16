import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, catchError, debounceTime, map, Observable, Subject, switchMap, tap, throwError } from 'rxjs';
import { PostDto } from '../../dto/post.dto';
import { PostService } from '../post/post.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PostStateService implements OnInit{
  private _postsSubject = new BehaviorSubject<PostDto[]>([]);
  private _loadingSubject = new BehaviorSubject<boolean>(false);
  
  public posts$ = this._postsSubject.asObservable();
  public isLoading$ = this._loadingSubject.asObservable();

  public posts:PostDto[] = [];
  private currentPage = 1;


  constructor(private _postService:PostService) { }

  ngOnInit():void{
    this.loadPosts();
  }
  
  setLoading(value:boolean){
    this._loadingSubject.next(value);
  }

  loadMore(){
    this.currentPage += 1;
    this.loadPosts();
  }

  loadPosts() {
    this.setLoading(true);
    this._postService.getPosts(this.currentPage).subscribe({
      next:response=>{
        this.posts = this.posts.concat(response);
        this._postsSubject.next(this.posts);
        this.setLoading(false)
      }
    })
  }
  
}
