import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { forkJoin, Subscription } from 'rxjs';
import { StoriesDto } from 'src/app/core/dto/stories.dto';
import { StoryDto } from 'src/app/core/dto/story.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';

@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.scss'],
  standalone:false
})
export class StoryComponent implements OnInit{
  username:String = ""

  groupedStories:StoriesDto[] = []
  storiesByUser:StoriesDto[]= [];
  extractedStories?:StoryDto[] = [];
  activeStoryList:StoriesDto[] = [];


  storyPreview:boolean = false;
  newStory:boolean = false;
  default:boolean = true;
  isStorySeen:boolean = false;

  selectedStoryIndex!:number 
  
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
   this.activeStoryList = this.groupedStories; 
   this.selectedStoryIndex = index; 
   this._dashboardState.setStoryPrewiew(true)
   this._stateService.setCurrentStoryState(index);
  }

  openUserStories(index: number){
    this.activeStoryList = this.storiesByUser;
    this.selectedStoryIndex = index; 
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
      forkJoin({
        groupedStories:this._storyService.getStories(this.username, false),
        storiesByUser:this._storyService.getStories(this.username, true)
      }).subscribe({
        next:({groupedStories, storiesByUser})=>{
          
          this.groupedStories = groupedStories;
    
          if(storiesByUser.length > 0){
            this.storiesByUser = storiesByUser;
            this.extractFromResponse(storiesByUser);
            console.log(storiesByUser)
          }
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
  private extractFromResponse(storiesByUser:StoriesDto[]){
      this.extractedStories = storiesByUser[0].stories
      if(  this.extractedStories != undefined &&  this.extractedStories.length > 0) this.default = false;
     
  }

}
