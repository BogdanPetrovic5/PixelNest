import { Component, Input, OnInit } from '@angular/core';
import { StoriesDto } from 'src/app/core/dto/stories.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';


@Component({
  selector: 'app-story-list',
  templateUrl: './story-list.component.html',
  styleUrls: ['./story-list.component.scss'],

})
export class StoryListComponent implements OnInit{
  @Input() storyList:StoriesDto[] = []
  @Input() userIndex!:number;
  marginStep = 56;
  margin:number = 90;

  counter:number = 1;
 
  constructor(
    private _storyState:StoryStateService,
    private _dashboardState:DashboardStateService
  ){}
  ngOnInit(): void {
    this.margin = this.margin - 56*this.userIndex
    this._storyState.currentStory$.subscribe({
      next:response=>{
        
        if(response > this.userIndex) this.margin = this.margin - 56;
        else if(response < this.userIndex) this.margin = this.margin + 56;
        if(response < 0 || response == this.storyList.length) this._dashboardState.setStoryPrewiew(false)

        this.userIndex = response
      }
     })


  }
}
