import { animate } from '@angular/animations';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { TimeInterval } from 'rxjs/internal/operators/timeInterval';
import { StoryDto } from 'src/app/core/dto/story.dto';

@Component({
  selector: 'app-story-preview',
  templateUrl: './story-preview.component.html',
  styleUrls: ['./story-preview.component.scss']
})
export class StoryPreviewComponent implements OnInit, OnDestroy{
    @Input() stories:StoryDto[] = [];  
    
    currentIndex = 0;
    storyCurrentLength:number = 0;
    targetValue = 100;
    storyDuration = 5000;
    interval = 4.16;
    step = this.targetValue / (this.storyDuration / 16.67);
    animationFrameId: any;

    storyInterval:any
    ngOnDestroy(): void {
      cancelAnimationFrame(this.animationFrameId);
    }
    ngOnInit(): void {
     this._initilizeInterval();
    }
    navigate(direction:string){
      if(direction === "left"){
        if(this.currentIndex - 1 >= 0){
          this.currentIndex -= 1
          this.storyCurrentLength = 0
          clearInterval(this.storyInterval)
        } 
      }else if(this.currentIndex + 1 <= this.stories.length){
        this.currentIndex += 1;
        this.storyCurrentLength = 0
        clearInterval(this.storyInterval)
      }
    }
    private _initilizeInterval(){
      
      this.storyCurrentLength = 0;

      const animate = () => {

        this.storyCurrentLength += this.step;

        if (this.storyCurrentLength >= this.targetValue) {

          this.storyCurrentLength = this.targetValue;
          this.storyCurrentLength = 0;
          
          this.currentIndex += 1;
          if (this.currentIndex >= this.stories.length) {
            cancelAnimationFrame(this.animationFrameId);
            return;
          }
        }
        this.animationFrameId = requestAnimationFrame(animate);
      };

      this.animationFrameId = requestAnimationFrame(animate);
    }
  
}
