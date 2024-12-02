import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { StoriesDto } from 'src/app/core/dto/stories.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';


@Component({
  selector: 'app-story-list',
  templateUrl: './story-list.component.html',
  styleUrls: ['./story-list.component.scss'],

})
export class StoryListComponent implements OnInit, OnDestroy{
  @Input() storyList:StoriesDto[] = []
  @Input() userIndex!:number;
  marginStep = 28;
  margin:number = 37.5;

  counter:number = 1;
  storySubscription: Subscription | undefined;
  constructor(
    private _storyState:StoryStateService,
    private _dashboardState:DashboardStateService
  ){}
  ngOnDestroy():void{
    this.margin = 37.5
    this.storySubscription?.unsubscribe();
    ///Check for bugs!
  }
  ngOnInit(): void {
    this.margin = this.margin - this.marginStep * this.userIndex
    this.storySubscription = this._storyState.currentStory$.subscribe({
        next:response=>{
          
          if(response > this.userIndex) this.margin = this.margin - this.marginStep;
          else if(response < this.userIndex) this.margin = this.margin + this.marginStep;
          if(response < 0 || response >= this.storyList.length) this._dashboardState.setStoryPrewiew(false)

          this.userIndex = response
        }
      })
  
  }
}
