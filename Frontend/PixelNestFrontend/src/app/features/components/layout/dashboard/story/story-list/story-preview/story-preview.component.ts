import { animate } from '@angular/animations';
import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
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
    storyDuration = 3000;
    interval = 4.16;
    step = this.targetValue / (this.storyDuration / 16.67);
    animationFrameId: any | undefined;
    storySubscription: Subscription | undefined;
  

    constructor(
      private _dashboardState:DashboardStateService,
      private _storyState:StoryStateService
    ){}
    ngOnInit(): void {
      console.log(`Initializing instance ${this.listIndex}`);
      this.currentIndex = 0;
      this.storyCurrentLength = 0;  
     
      
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
      console.log(`Destroying instance ${this.listIndex}`);
  
      this._stopAnimation();
      this.storySubscription?.unsubscribe();
      this._storyState.resetCurrentState();
    }

   ngOnChanges(changes: SimpleChanges): void {
     
   }
    navigate(direction:string){
      
      if(direction === "left"){
        if(this.currentIndex - 1 >= 0){
          this.currentIndex -= 1
          this.storyCurrentLength = 0
        
        } 
      }else if(this.currentIndex + 1 <= this.stories.length){
        this.currentIndex += 1;
        this.storyCurrentLength = 0
      
      }
    }
    close() {
      this._stopAnimation();
    
      this._dashboardState.setStoryPrewiew(false);
    }
    private animate = () => {
        
      this.storyCurrentLength += this.step;
      if (this.storyCurrentLength >= this.targetValue) {
        console.log(this.targetValue)
        this.storyCurrentLength = 0;
        this.currentIndex += 1;
        if (this.currentIndex >= this.stories.length) {
          this.currentIndex = 0;
          this._stopAnimation()
          this._storyState.setCurrentStoryState(1);
          
          return;
        }
        
      }
      this.animationFrameId = requestAnimationFrame(this.animate);
      
    };




    private _startAnimation() {
      this.storyCurrentLength = 0;
      this.currentIndex = 0;


      this.animate();
   

    }

    private _stopAnimation(){
      if(this.animationFrameId){
        cancelAnimationFrame(this.animationFrameId);
        this.animationFrameId = undefined
      }
    }


 
  
}
