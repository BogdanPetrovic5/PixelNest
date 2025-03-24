import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Subject, takeUntil, tap } from 'rxjs';
import { PostDto } from 'src/app/core/dto/post.dto';
import { PostService } from 'src/app/core/services/post/post.service';

@Component({
  selector: 'app-post-view',
  templateUrl: './post-view.component.html',
  styleUrls: ['./post-view.component.scss']
})
export class PostViewComponent implements OnInit{
  constructor(
    private _postService:PostService,
    private _route:ActivatedRoute,
    private _cdr:ChangeDetectorRef,
    private _router:Router
  ){}
  post?:PostDto
  
  postID:string = "-1";
  private destroy$ = new Subject<void>();
  ngOnInit(): void {
    this._route.paramMap
       .pipe(
         takeUntil(this.destroy$), 
         tap(params => {
            let postID = params.get('postID')
            if(postID) this.postID = postID
           
            this._getPostData(this.postID);
         })
       ).subscribe({
        error: err => {
          console.error('Error during profile setup:', err);
        }
      });
  }
  navigate(){
    this._router.navigate(["Notifications"])
  }
  private _getPostData(postID:string){
   
    this._postService.getSinglePost(postID).subscribe({
      next:response=>{
        this.post = response
    
        this._cdr.detectChanges()
      }
    })
  }
}
