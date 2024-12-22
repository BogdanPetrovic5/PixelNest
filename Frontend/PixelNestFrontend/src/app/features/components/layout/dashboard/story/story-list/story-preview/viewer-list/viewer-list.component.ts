import { Component, Input, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ViewersDto } from 'src/app/core/dto/viewers.dto';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-viewer-list',
  templateUrl: './viewer-list.component.html',
  styleUrls: ['./viewer-list.component.scss']
})
export class ViewerListComponent implements OnInit{
  @Input() storyID?:number;
  viewers:ViewersDto[] = []
  anim:boolean = false;
  subscriptions:Subscription = new Subscription();
  constructor(private _storyService:StoryService, private _userSession:UserSessionService, private _storyState: StoryStateService){

  }
  ngOnInit(): void {
    this.initilizeComponent()
  }
  initilizeComponent(){
    this.subscriptions.add(
      this._storyState.viewerListAnim$.subscribe({
        next:response=>{
          
          this.anim = response;
         
        }
      })
    )

    let username = this._userSession.getFromCookie("username");
    console.log("Username: ", username);
    if(this.storyID != null || this.storyID != undefined){
      this._storyService.getViewers(username, this.storyID).subscribe({
        next:response=>{
          this.viewers = response;
          console.log(this.viewers);
        }
      })
    }
  }
  close(event:any){
    if( (event.additionalEvent === 'pandown') ){
      
        this._storyState.setViewerListAnim(false);
        setTimeout(()=>{
          this._storyState.setIsViewerList(false);
        }, 1000)
      
    }
   
  }
  
}
