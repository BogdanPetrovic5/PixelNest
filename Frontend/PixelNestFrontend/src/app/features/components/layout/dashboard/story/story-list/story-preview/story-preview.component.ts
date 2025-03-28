import { animate } from '@angular/animations';
import { ChangeDetectorRef, Component, Input, NgZone, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';

import { Subscription } from 'rxjs';

import { StoryDto } from 'src/app/core/dto/story.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { environment } from 'src/environments/environment.development';
import 'hammerjs';
@Component({
  selector: 'app-story-preview',
  templateUrl: './story-preview.component.html',
  styleUrls: ['./story-preview.component.scss']
})
export class StoryPreviewComponent implements OnInit, OnDestroy{
    @Input() stories:StoryDto[] = [];  
    @Input() listIndex!:number 
    userIndex:number = 0;
    currentIndex = 0;

    storyCurrentLength:number = 0;
    targetValue = 100;
    storyDuration = 15000;
    interval = 4.16;
    step = this.targetValue / (this.storyDuration / 33.67);
    animationFrameId: any | undefined;
    subscriptions: Subscription = new Subscription();

    username:string = "";
    baseUrl:string = ""
    isViewerList:boolean = false;

    constructor(
      private _dashboardState:DashboardStateService,
      private _storyState:StoryStateService,
      private _storyService:StoryService,
      private _cdr: ChangeDetectorRef, 
      private _userService:UserSessionService
     
    ){}
    
    ngOnInit(): void {    
      console.log(this.stories)
      this.baseUrl = environment.blobStorageBaseUrl;
      this.username = this._userService.getFromCookie("username")
      this.subscriptions?.add(
        this._storyState.currentStory$.subscribe({
          next:response=>{
            this.userIndex = response
            if(this.userIndex == this.listIndex) {
              if(this.stories != undefined && !this.stories?.[this.currentIndex].seenByUser) this._markStoryAsSeen(this.stories[this.currentIndex].storyID);
             
              this._startAnimation()
            }else {
              this._stopAnimation()
            }
          }
         })
      )
      this.subscriptions?.add(
        this._storyState.isViewerList$.subscribe({
          next:response=>{
            this.isViewerList = response;
            if(this.isViewerList == true) this._pauseAnimation();
            else this._resumeAnimation();
          }
        })
      )
    }

    ngOnDestroy(): void {
      this._stopAnimation();
     
      this.subscriptions?.unsubscribe();
      this._storyState.resetCurrentState();
     
    }

    handleViwersTab(){
      if(this._storyState.getViewerListState() == true){
       
        this._storyState.setViewerListAnim(false);
        setTimeout(()=>{
          this._storyState.setIsViewerList(!this._storyState.getViewerListState());
        }, 1000)
      }else {
       
        this._storyState.setViewerListAnim(true);
        this._storyState.setIsViewerList(!this._storyState.getViewerListState());
      }
      
    }
    navigate(direction:string){
      if(direction == "left"){
        if(this.currentIndex - 1 >= 0){
          this._updateCurrentIndex(-1);
        
        }else{
          this._resetParameters();
          this._stopAnimation();
          this._storyState.setCurrentStoryState(-1);
        }
      }

      if(direction == "right"){
        if(this.currentIndex + 1 < this.stories!.length){
         this._updateCurrentIndex(1)
        }else{
          this._resetParameters();
          this._stopAnimation();
          this._storyState.setCurrentStoryState(1);
        }
      }
    }

  
    close() {
      this._stopAnimation();
      this._dashboardState.setStoryPrewiew(false);
    }



    private _updateCurrentIndex(value:number){
      this.storyCurrentLength = 0;
      this.currentIndex += value;
     
      if(this.currentIndex < this.stories!.length && !this.stories![this.currentIndex].seenByUser) this._markStoryAsSeen(this.stories![this.currentIndex].storyID);
    }

    private animate = () => {
        
      this.storyCurrentLength += this.step;
      if (this.storyCurrentLength >= this.targetValue) {
     
        this._updateCurrentIndex(1);
     
        if (this.currentIndex >= this.stories!.length) {
          this._resetParameters();
          this._stopAnimation()
          this._storyState.setCurrentStoryState(1);
          
          return;
        }
        
      }
      this.animationFrameId = requestAnimationFrame(this.animate);
    };

    private _resumeAnimation() {
      if (!this.animationFrameId) {
        this.animationFrameId = requestAnimationFrame(this.animate); 
      }
    }

    private _pauseAnimation(){
      if (this.animationFrameId) {
        cancelAnimationFrame(this.animationFrameId); 
        this.animationFrameId = null;
      }
    }

    private _markStoryAsSeen(storyID:string){
      
      this._storyService.marStoryAsSeen(storyID).subscribe({
        next:response=>{
          
        }
        ,error:error =>{
          console.log(error)
        }
      })
    }

    private _resetParameters(){
      this.storyCurrentLength = 0;
      this.currentIndex = 0;
    }
    
    private _startAnimation() {
      this._resetParameters();
      this.animate();
    }

    private _stopAnimation(){
      if(this.animationFrameId){
        cancelAnimationFrame(this.animationFrameId);
        this.animationFrameId = undefined
      }
    }


 
  
}
