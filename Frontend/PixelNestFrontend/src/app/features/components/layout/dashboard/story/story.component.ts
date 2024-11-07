import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Subscription } from 'rxjs';
import { StoriesDto } from 'src/app/core/dto/stories.dto';
import { StoryDto } from 'src/app/core/dto/story.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';

@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.scss']
})
export class StoryComponent implements OnInit{
  username:String = ""
  groupedStories:StoriesDto[] = []
  storiesByUser:StoryDto[] = [];
  storyPreview:boolean = false;
  newStory:boolean = false;
  subscription:Subscription = new Subscription();

  constructor(
    private _storyService:StoryService,
    private _cookieService:CookieService,
    private _dashboardState:DashboardStateService,
    private _stateService:StoryStateService
  ){}  
  ngOnInit(): void {
    this.username = this._cookieService.get("username");
    this._initilizeComponent();
    
  }

  openNewStory(){
   
    this._dashboardState.setIsNewStoryTabOpen(true)
  }

  openStories(index: number){
    
   this._dashboardState.setStoryPrewiew(true)
   this._stateService.setCurrentStoryState(index);
  }

  private _initilizeComponent() {
    this._initilizeSubscriptions();
  }
  private _initilizeSubscriptions() {
    this.subscription.add(
      this._dashboardState.newStoryTab$.subscribe({
        next:response=>{
          this.newStory = response
        }
      })
    )
    this.subscription.add( 
        this._storyService.getStories(this.username).subscribe({
        next:response=>{
          this.groupedStories = response
          console.log(this.groupedStories)
        }
      })      
    )

    this.subscription.add(
        this._dashboardState.storyPreview$.subscribe({
          next:response=>{
            this.storyPreview = response
          }
        })
    )
  }

}
