import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardComponent } from './dashboard.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { PostStateService } from 'src/app/core/services/states/post-state.service';
import { of } from 'rxjs';
import { PostDto } from 'src/app/core/dto/post.dto';
import { mockPostDto } from 'src/app/testing/mocks/postMock.dto';
import { TopNavigationComponent } from 'src/app/shared/components/top-navigation/top-navigation.component';
import { StoryComponent } from './story/story.component';
import { FeedComponent } from 'src/app/shared/components/feed/feed.component';
import { NotificationIconComponent } from 'src/app/shared/components/notification-icon/notification-icon.component';
import { InboxIconComponent } from 'src/app/shared/components/inbox-icon/inbox-icon.component';
import { ProfileImageComponent } from 'src/app/shared/components/profile-image/profile-image.component';
import { TopNavigationStub } from 'src/app/testing/stubs/top-navigation-stub';
import { StoryStub } from 'src/app/testing/stubs/story-stub';
import { FeedStub } from 'src/app/testing/stubs/feed-stub';
import { CacheService } from 'src/app/core/services/cache/cache.service';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  const mockCacheService = {
    checkCache:jasmine.createSpy('checkCache').and.returnValue(of(true))
  }

  const mockPostDtoArray = [mockPostDto];
  const mockPostStateService = {
    feedPosts: [mockPostDto],
    feedPosts$: of([mockPostDto]),
    isLoading$: of(false),
    loadPosts: jasmine.createSpy('loadPosts').and.returnValue(of(mockPostDtoArray)),
    setPosts: jasmine.createSpy('setPosts'),
    resetFeed: jasmine.createSpy('resetFeed')
  }

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardComponent, TopNavigationStub,
        StoryStub,
        FeedStub,],
      imports:[HttpClientTestingModule],
      providers:[
        {
          provide:PostStateService, useValue:mockPostStateService,
        },
        {
          provide:CacheService, useValue:mockCacheService
        }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
   
    fixture.detectChanges();
  });
  fit('is cache checked and load posts called', async ()=>{
      component.ngOnInit(); 
      await fixture.whenStable();
      expect(mockCacheService.checkCache).toHaveBeenCalled();
      expect(mockPostStateService.loadPosts).toHaveBeenCalled();
  })
  
  fit('should create', () => {
    expect(component).toBeTruthy();
  });

}); 

