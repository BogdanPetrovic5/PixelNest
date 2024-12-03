import { animate } from '@angular/animations';
import { ChangeDetectorRef, Component, Input, NgZone, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Subscription } from 'rxjs';

import { StoryDto } from 'src/app/core/dto/story.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';

@Component({
  selector: 'app-story-preview',
  templateUrl: './story-preview.component.html',
  styleUrls: ['./story-preview.component.scss']
})
export class StoryPreviewComponent implements OnInit, OnDestroy{
    @Input() stories:StoryDto[] | undefined = [];  
    @Input() listIndex!:number 
    userIndex:number = 0;
    currentIndex = 0;

    storyCurrentLength:number = 0;
    targetValue = 100;
    storyDuration = 15000;
    interval = 4.16;
    step = this.targetValue / (this.storyDuration / 33.67);
    animationFrameId: any | undefined;
    storySubscription: Subscription | undefined;
  

    constructor(
      private _dashboardState:DashboardStateService,
      private _storyState:StoryStateService,
      private _storyService:StoryService,
      private _cdr: ChangeDetectorRef,
      private _cookieService:CookieService
     
    ){}

    ngOnInit(): void {    
     
      this.storySubscription = this._storyState.currentStory$.subscribe({
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
    }

    ngOnDestroy(): void {
      this._stopAnimation();
     
      this.storySubscription?.unsubscribe();
      this._storyState.resetCurrentState();
     
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
      console.log(this.currentIndex, this.stories!.length);
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
  
    private _markStoryAsSeen(storyID:number){
      const username = this._cookieService.get("username");
      this._storyService.marStoryAsSeen(storyID, username).subscribe({
        next:response=>{
          console.log(response)
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
