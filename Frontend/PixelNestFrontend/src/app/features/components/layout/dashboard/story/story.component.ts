import { AfterViewChecked, AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { forkJoin, Subscription, switchMap } from 'rxjs';
import { StoriesDto } from 'src/app/core/dto/stories.dto';
import { StoryDto } from 'src/app/core/dto/story.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';
import 'hammerjs';
@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.scss'],
  standalone:false
})
export class StoryComponent implements OnInit{
  @ViewChild('storyList', { static: false }) storyList!: ElementRef<HTMLDivElement>
  @ViewChild('storyListWrapper', { static: false }) storyListWrapper!: ElementRef<HTMLDivElement>
  username:string = ""

  groupedStories:StoriesDto[] = []
  storiesByUser:StoriesDto[]= [];
  extractedStories:StoryDto[] = [];
  activeStoryList:StoriesDto[] = [];


  storyPreview:boolean = false;
  newStory:boolean = false;
  default:boolean = true;
  isStorySeen:boolean = false;

  selectedStoryIndex!:number 
  marginLeft:number = 0;
  stepSize:number = 0;
  currentPage = 1;
  subscription:Subscription = new Subscription();


  constructor(
    private _storyService:StoryService,
    private _cookieService:CookieService,
    private _dashboardState:DashboardStateService,
    private _stateService:StoryStateService,
    private _storyState:StoryStateService,
    private changeDetectorRef: ChangeDetectorRef
  ){}  
  ngOnDestroy():void{
    this.subscription.unsubscribe();
    this._storyState.setStories();
  }
  ngOnInit(): void {
    this.username = this._cookieService.get("username");
    this._initilizeComponent();
    
  }

  onSwipeLeft() {
    this.changeDetectorRef.detectChanges();
    const limit = this.storyListWrapper.nativeElement.getBoundingClientRect().right
    const right = this.storyList.nativeElement.getBoundingClientRect().right;
    if (right >= limit) {
      this.marginLeft -= this.stepSize;
    }
  }
  onSwipeRight() {
    if (this.marginLeft < 0) {
      this.marginLeft += this.stepSize;
    }
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
  private initializeStepSize() {
    const containerWidth = this.storyList.nativeElement.offsetWidth;
    const numberOfVisibleStories = Math.floor(containerWidth / this._getStoryBoxWidth());
    this.stepSize = (100+numberOfVisibleStories) / numberOfVisibleStories;
    
  }
  private _getStoryBoxWidth(): number{
    const storyBox = this.storyList.nativeElement.querySelector('.user-box');
    const width = storyBox ? storyBox.getBoundingClientRect().width : 0;
    
    return width;
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
    // this.subscription.add(
    //   forkJoin({
    //     groupedStories:this._storyState.fetchStories(this.username, false),
    //     storiesByUser:this._storyState.fetchStories(this.username, true)
    //   }).subscribe({
    //     next:({groupedStories, storiesByUser})=>{
          
    //       this.groupedStories = groupedStories;
    //       setTimeout(() => {
    //         this.initializeStepSize();
    //       }, 0);
    //       if(storiesByUser.length > 0){
    //         this.storiesByUser = storiesByUser;
            
    //         this.extractFromResponse(storiesByUser);
            
    //       }
    //     }
    //   })
    // )
    this._storyState.fetchStories(this.username, false);
    
    this.subscription.add(
      this._storyState.stories$.subscribe({
        next:response=>{
          
          this.groupedStories = response
          console.log(this.groupedStories);
          setTimeout(()=>{
            this.initializeStepSize()
          },0)
          
        }
      })
    )
    this._storyState.fetchCurrentUserStories(this.username, true);
    this.subscription.add(
      this._storyState.storyListByUser$.subscribe({
        next:response=>{
          
            this.storiesByUser = response;
            console.log(this.storiesByUser);
            
            this.extractFromResponse(this.storiesByUser);
            
           
          
        }
      })
    )
    // this.subscription.add(
    //   this._storyState.currentPage$
    //     .pipe(
    //       switchMap((page) =>
    //         forkJoin({
    //           groupedStories: this._storyService.getStories(this.username, false, page),
    //           storiesByUser: this._storyService.getStories(this.username, true, 1),
    //         })
    //       )
    //     )
    //     .subscribe({
    //       next: ({ groupedStories, storiesByUser }) => {
    //         this.groupedStories = this.groupedStories.concat(groupedStories);
            
    //         if (storiesByUser.length > 0) {
    //           this.storiesByUser = storiesByUser;
    //           this.extractFromResponse(storiesByUser);
              
    //         }
    //       },
    //     })
    // );




    this.subscription.add(
        this._dashboardState.storyPreview$.subscribe({
          next:response=>{
            this.storyPreview = response
          }
        })
    )
  }
  private extractFromResponse(storiesByUser:StoriesDto[]){
      if(storiesByUser != undefined && storiesByUser[0]?.stories.length > 0){
        this.extractedStories = storiesByUser[0].stories
        if( this.extractedStories != undefined &&  this.extractedStories.length > 0) this.default = false;
      }
     
     
  }

}
