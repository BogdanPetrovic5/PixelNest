import { animate } from '@angular/animations';
import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { flush } from '@angular/core/testing';
import { TimeInterval } from 'rxjs/internal/operators/timeInterval';
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
    animationFrameId: any;

  

    constructor(
      private _dashboardState:DashboardStateService,
      private _storyState:StoryStateService
    ){}
    ngOnDestroy(): void {
      cancelAnimationFrame(this.animationFrameId);
      this._storyState.resetCurrentState();
    }
    ngOnInit(): void {
      this.currentIndex = 0;
      this.storyCurrentLength = 0;  
     
    
      this._storyState.currentStory$.subscribe({
        next:response=>{
          this.userIndex = response
         
          if(this.userIndex == this.listIndex) {
         
            
            this.animationFrameId = requestAnimationFrame(animate);
            this._initilizeInterval();
          }else {
           
            cancelAnimationFrame(this.animationFrameId);
          }
        }
       })
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
    close(){
      this._dashboardState.setStoryPrewiew(false);
    }
    private _initilizeInterval(){
      
      this.storyCurrentLength = 0;
      this.currentIndex = 0
      const animate = () => {
        
        this.storyCurrentLength += this.step;

        if (this.storyCurrentLength >= this.targetValue) {

          this.storyCurrentLength = this.targetValue;
          this.storyCurrentLength = 0;
          
          this.currentIndex += 1;
          if (this.currentIndex >= this.stories.length) {
            this.currentIndex = 0;
            cancelAnimationFrame(this.animationFrameId);
            this._storyState.setCurrentStoryState(1);
            // this._dashboardState.setStoryPrewiew(false)
            return;
          }
        }
        this.animationFrameId = requestAnimationFrame(animate);
      };

      this.animationFrameId = requestAnimationFrame(animate);
    }
  
}
