import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PostService } from 'src/app/core/services/post/post.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';

@Component({
  selector: 'app-delete-dialog',
  templateUrl: './delete-dialog.component.html',
  styleUrls: ['./delete-dialog.component.scss']
})
export class DeleteDialogComponent {
  @Input() postID!:number
  @Output() closeDeleteDialog: EventEmitter<void> = new EventEmitter<void>();
  anim:boolean = false;
  constructor(
    
      private _postService:PostService,
      
      private _postState:PostStateService,
      private _lottie:LottieStateService
    ){
     
    }
  choose(choice:'yes'| 'no'){
    if(choice === 'yes'){
      this._lottie.setIsInitialized(true);
      this._postService.deletePost(this.postID).subscribe({
        next:response=>{
         
          this._postState.setPosts([]);
          this._postState.loadPosts(1);
          this._lottie.setIsInitialized(false);
        },
        error:error=>{
          console.error(error);
        }
      })
    }else this.close();
    
  }

  close(){
    this.anim = true;
    setTimeout(() => {
      this.closeDeleteDialog.emit()
      this.anim = false
    }, 500);
    
  }
}
