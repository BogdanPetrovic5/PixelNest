import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { catchError, debounceTime, switchMap, throwError } from 'rxjs';
import { PostService } from 'src/app/core/services/post/post.service';
import { PostDto } from 'src/app/core/dto/post.dto';
import { Subject } from 'rxjs';
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

  isEmpty:boolean = false;

  searchSubject: Subject<void> = new Subject<void>();
  constructor(private _postService: PostService){

  }
  ngOnInit(): void {
    console.log(this.currentPage)
    this._initilizeApp();
  }
  
  loadPosts(){
    this.isLoading = true;
    this.searchSubject.pipe(
      debounceTime(500),
      switchMap(()=> this._postService.getPosts(this.currentPage)),
      catchError((error: HttpErrorResponse) => {
        console.log(error);
        return throwError(() => error);
      })
    ).subscribe(response => {
      if (response.length < 5) this.isEmpty = true;
      console.log(response)
      this.temporalResponse = response;
      this.posts = this.posts.concat(this.temporalResponse);
  
      console.log(this.posts);
      this.isLoading = false;
    });
    this.searchSubject.next();
    // this._postService.getPosts(this.currentPage)
    // .pipe(
    //   catchError((error:HttpErrorResponse)=>{
    //     console.log(error)
    //     return throwError(()=>error);
    //   })
    // ).subscribe(response =>{
    //   if(response.length < 5) this.isEmpty = true;
    //   this.temporalResponse = response
    //   this.posts = this.posts.concat(this.temporalResponse);
    //   console.log(this.posts);
    //   this.isLoading = false;
     
    // })
  }

  loadMore(event:any){
    
    if(!this.isEmpty){
      const scrollElement = event.target;
      if ((scrollElement.offsetHeight + 0) + scrollElement.scrollTop >= scrollElement.scrollHeight) {
       
        this.currentPage += 1;
        
        this.loadPosts()
      }
    }
   
  }
 private _initilizeApp(){
    this.loadPosts()
  }
}
