import { AfterViewChecked, AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { forkJoin, last, Subscription, switchMap } from 'rxjs';
import { StoriesDto } from 'src/app/core/dto/stories.dto';
import { StoryDto } from 'src/app/core/dto/story.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { StoryStateService } from 'src/app/core/services/states/story-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';
import 'hammerjs';
import { math } from '@maptiler/sdk';
import { ProfileStateService } from 'src/app/core/services/states/profile-state.service';
@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.scss'],
  standalone:false
})
export class StoryComponent implements OnInit, OnDestroy, AfterViewInit{
  @ViewChild('storyList', { static: false }) storyList!: ElementRef<HTMLDivElement>
  @ViewChild('storyListWrapper', { static: false }) storyListWrapper!: ElementRef<HTMLDivElement>
  @ViewChildren('userBox') userBoxes!: QueryList<ElementRef>;

  username:string = ""
  stringUrl:string = ""
  groupedStories:StoriesDto[] = []
  storiesByUser:StoriesDto[]= [];
  extractedStories:StoryDto[] = [];
  activeStoryList:StoriesDto[] = [];


  storyPreview:boolean = false;
  newStory:boolean = false;
  default:boolean = true;
  isStorySeen:boolean = false;
  isDragging:boolean = false;

  selectedStoryIndex!:number 
  marginLeft:number = 0;
  stepSize:number = 0;
  currentPage = 1;
  subscription:Subscription = new Subscription();
  minMargin:number = 0;
  startX:number = 0;
  currentMargin:number = 0;
  containerWidth:number = 0;
  clientGuid:string = ""
  constructor(
    private _storyService:StoryService,
    private _cookieService:CookieService,
    private _dashboardState:DashboardStateService,
    private _stateService:StoryStateService,
    private _storyState:StoryStateService,
    private changeDetectorRef: ChangeDetectorRef,
    private _profileState:ProfileStateService
  ){}  
  ngOnDestroy():void{
    this.subscription.unsubscribe();
    this._storyState.setStories();
  }
  ngOnInit(): void {
    this.username = this._cookieService.get("username");
    this.clientGuid = this._cookieService.get("sessionID")
    this._initializeComponent();
    
  }
  ngAfterViewInit():void{
    
      
   
  }

  calculateContainerWidth(): void {
    if (this.userBoxes.last) {
      this.containerWidth = this.userBoxes.last.nativeElement.offsetWidth;
    } else {
      this.containerWidth = 30; 
    }
 
  }


  onPanStart(event:any){
    this.isDragging = true;
    this.startX = event.touches[0].clientX;
    this.currentMargin = this.marginLeft;
  }
  onPanMove(event:any){
      if(!this.isDragging) return
      
      const delta = (event.touches[0].clientX - this.startX) * 1.3
      const potentialMargin = this.currentMargin + delta;

      const maxMargin = 0;
      const minMarginResult = this.calculateMinMargin();
      if (minMarginResult === false && delta < 0) {
        return; 
      }

      this.marginLeft = Math.max(this.minMargin, Math.min(maxMargin, potentialMargin));
      this.updateSliderTransform(false);
  }
  calculateMinMargin(){
    const followingList = document.getElementById('following-list')?.getBoundingClientRect();
    const lastBox = this.userBoxes.last.nativeElement.getBoundingClientRect();
    
    if (!followingList || !lastBox) {
      
      return -(this.groupedStories.length - 1) * this.containerWidth; 
    }

    const followingListRight = parseFloat(followingList.right.toFixed(1));
    const lastBoxRight = parseFloat(lastBox.right.toFixed(1));
  
 
    if (lastBoxRight <= followingListRight) {
    
      return false;
    }
  
    this.minMargin = -(this.groupedStories.length - 1) * this.containerWidth;
    return true;
  }
  onPanEnd(event: any) {

    this.isDragging = false;
    this.updateSliderTransform(true);
}
  getImageWidth(): number {
    const imageWrapper = document.querySelector('.user-box') as HTMLElement;
    
    return imageWrapper ? imageWrapper.offsetWidth : 0;
  }


  updateSliderTransform(smooth: boolean) {
    const slider = document.getElementById(`following`);
    if (slider) {
     
      slider.style.transition = smooth ? 'transform 0.2s ease-out' : 'none';
      slider.style.transform = `translateX(${this.marginLeft}px)`;
    }
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

  private _initializeComponent() {
    this._initializeSubscriptions();
  }

  private _initializeSubscriptions() {
    this.subscription.add(
      this._dashboardState.newStoryTab$.subscribe({
        next:response=>{
          this.newStory = response
        }
      })
    )
    this._storyState.fetchStories(false);
    
    this.subscription.add(
      this._storyState.stories$.subscribe({
        next:response=>{
          this.groupedStories = response
          this.groupedStories.sort((a, b) => {
            const aHasUnread = a.stories.some(story => story.seenByUser === false);
            const bHasUnread = b.stories.some(story => story.seenByUser === false);
            
          
            if (aHasUnread === bHasUnread) return 0;
            return aHasUnread ? -1 : 1;
          });
          setTimeout(()=>{
            this.initializeStepSize()
            setTimeout(() => this.calculateContainerWidth());
          },0)
          
        }
      })
    )
    this._storyState.fetchCurrentUserStories(true);
    this.subscription.add(
      this._storyState.storyListByUser$.subscribe({
        next:response=>{
          
            this.storiesByUser = response;
            
            this.extractFromResponse(this.storiesByUser);
            
           
          
        }
      })
    )
    this.subscription.add(
      this._profileState.currentProfileUrl$.subscribe({
        next:response =>{
          this.stringUrl = response
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
      if(storiesByUser != undefined && storiesByUser[0]?.stories.length > 0){
        this.extractedStories = storiesByUser[0].stories
        if( this.extractedStories != undefined &&  this.extractedStories.length > 0) this.default = false;
      }
     
     
  }

}
