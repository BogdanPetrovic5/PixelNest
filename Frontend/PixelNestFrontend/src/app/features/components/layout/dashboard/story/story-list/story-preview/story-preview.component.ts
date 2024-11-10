import { animate } from '@angular/animations';
import { ChangeDetectorRef, Component, Input, NgZone, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';

import { StoryDto } from 'src/app/core/dto/story.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';

@Component({
  selector: 'app-story-preview',
  templateUrl: './story-preview.component.html',
  styleUrls: ['./story-preview.component.scss']
})
export class StoryPreviewComponent implements OnInit, OnDestroy, OnChanges{
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
    storySubscription: Subscription | undefined;
  

    constructor(
      private _dashboardState:DashboardStateService,
      private _storyState:StoryStateService,
      private _cdr: ChangeDetectorRef,
     
    ){}
    ngOnInit(): void {    
      this.storySubscription = this._storyState.currentStory$.subscribe({
        next:response=>{
          this.userIndex = response
          
          if(this.userIndex == this.listIndex) {
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

    ngOnChanges(changes: SimpleChanges): void {
      
    }  
    navigate(direction:string){
      if(direction == "left"){
        if(this.currentIndex - 1 >= 0){
          this.currentIndex -= 1
          this.storyCurrentLength = 0
        
        }else{
          this._resetParameters();
          this._stopAnimation();
          
          
          this._storyState.setCurrentStoryState(-1);
        }
      }

      if(direction == "right"){
        if(this.currentIndex + 1 < this.stories.length){
          this.currentIndex += 1;
          this.storyCurrentLength = 0;
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
    private animate = () => {
        
      this.storyCurrentLength += this.step;
      if (this.storyCurrentLength >= this.targetValue) {
       
        this.storyCurrentLength = 0;
        this.currentIndex += 1;
        if (this.currentIndex >= this.stories.length) {
          this._resetParameters();
          this._stopAnimation()
          this._storyState.setCurrentStoryState(1);
          
          return;
        }
        
      }
      this.animationFrameId = requestAnimationFrame(this.animate);
      
    };



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
