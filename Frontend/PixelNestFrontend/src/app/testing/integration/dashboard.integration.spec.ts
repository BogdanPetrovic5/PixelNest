import { HttpClientTestingModule, HttpTestingController } from "@angular/common/http/testing"
import { ComponentFixture, TestBed } from "@angular/core/testing"
import { RouterTestingModule } from "@angular/router/testing"
import { BehaviorSubject, of } from "rxjs"
import { CacheService } from "src/app/core/services/cache/cache.service"
import { IndexedDbService } from "src/app/core/services/indexed-db/indexed-db.service"
import { DashboardStateService } from "src/app/core/services/states/dashboard-state.service"
import { PostStateService } from "src/app/core/services/states/post-state.service"
import { UserSessionService } from "src/app/core/services/user-session/user-session.service"
import { DashboardComponent } from "src/app/features/components/layout/dashboard/dashboard.component"
import { mockPostDto } from "../mocks/postMock.dto"
import { environment } from "src/environments/environment.development"
import { TopNavigationStub } from "../stubs/top-navigation-stub"
import { StoryStub } from "../stubs/story-stub"
import { FeedStub } from "../stubs/feed-stub"
import { LottieLoadingComponent } from "src/app/shared/components/lottie-loading/lottie-loading.component"
import { SharedModule } from "src/app/shared/shared.module"

describe('DashboardComponent integration test', () =>{
    let component:DashboardComponent
    let fixture:ComponentFixture<DashboardComponent>
    
    let httpMock:HttpTestingController;
    
    beforeEach(async ()=>{
        await TestBed.configureTestingModule({
            declarations:[
                DashboardComponent,
               StoryStub
                
            ],
            imports:[
                HttpClientTestingModule, 
                RouterTestingModule,
                SharedModule
            ],
            providers:[
                PostStateService,
                DashboardStateService,
                UserSessionService,
                IndexedDbService,
                CacheService 
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(DashboardComponent);
        component = fixture.componentInstance;
        httpMock = TestBed.inject(HttpTestingController);

        fixture.detectChanges();
    })

    
    fit("it should call apis and load posts", async ()=>{
        const cacheReq = httpMock.expectOne(`${environment.apiUrl}/api/post/cache/state`)
        expect(cacheReq.request.method).toBe('GET');
        cacheReq.flush({isChanged:true})

        const mockPosts = [mockPostDto];
        const postReq = httpMock.expectOne(`${environment.apiUrl}/api/post/posts?page=1`);
        expect(postReq.request.method).toBe('GET');

        postReq.flush(mockPosts)
        fixture.detectChanges();

        expect(component.posts.length).toBe(1);
        expect(component.posts[0].postDescription).toBe(mockPostDto.postDescription);
       
    })
    afterEach(() => {
    });
})