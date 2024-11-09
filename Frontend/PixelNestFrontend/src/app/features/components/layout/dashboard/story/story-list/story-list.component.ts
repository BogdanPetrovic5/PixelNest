import { Component, Input, OnInit } from '@angular/core';
import { StoriesDto } from 'src/app/core/dto/stories.dto';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';


@Component({
  selector: 'app-story-list',
  templateUrl: './story-list.component.html',
  styleUrls: ['./story-list.component.scss'],

})
export class StoryListComponent implements OnInit{
  @Input() storyList:StoriesDto[] = []
  userIndex:number = 0;
  margin:number = 90;

  counter:number = 1;
 
  constructor(private _storyState:StoryStateService){}
  ngOnInit(): void {
    
    this._storyState.currentStory$.subscribe({
      next:response=>{
        this.counter += 1;
        if(response > this.userIndex) this.margin = this.margin - 56;
        this.userIndex = response
      }
     })


  }
}
