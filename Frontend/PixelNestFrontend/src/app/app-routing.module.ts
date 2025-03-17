import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './features/components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from './features/components/authentication-page/authentication-page.component';
import { LoginFormComponent } from './features/components/authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './features/components/authentication-page/register-form/register-form.component';
import { FeedComponent } from './shared/components/feed/feed.component';
import { DashboardComponent } from './features/components/layout/dashboard/dashboard.component';
import { AuthGuard } from './core/guards/auth.guard';
import { DashboardGuard } from './core/guards/dashboard.guard';
import { ProfileComponent } from './features/components/layout/profile/profile.component';
import { LocationComponent } from './shared/components/location/location.component';
import { LayoutComponent } from './features/components/layout/layout.component';
import { SearchComponent } from './features/components/layout/search/search.component';
import { InboxComponent } from './features/components/layout/inbox/inbox.component';
import { ChatComponent } from './shared/components/chat/chat.component';
import { UnauthorizedComponent } from './features/components/unauthorized/unauthorized.component';
import { NotificationComponent } from './features/components/layout/notification-modal/notification.component';
import { NotificationsComponent } from './features/components/layout/notifications/notifications.component';
import { PostComponent } from './shared/components/post/post.component';
import { PostViewComponent } from './features/components/layout/post-view/post-view.component';


const routes: Routes = [
  {path:'',redirectTo:"Unauthorized", pathMatch:'full'},
  {path:"Unauthorized", component:UnauthorizedComponent},
  {path:"Get Started", component:LandingPageComponent},
  {
    path:"Authentication", 
    component:AuthenticationPageComponent,
    children: [
      { path: '', redirectTo: 'Register', pathMatch: 'full' },
      { path: 'Register', component: RegisterFormComponent,canActivate:[AuthGuard] },
      { path: 'Login', component: LoginFormComponent,canActivate:[AuthGuard] }
    ],
    canActivate:[AuthGuard]
  },
  {
    path:"",
    component:LayoutComponent,
    children:[
      {
        path:"Dashboard", 
        component:DashboardComponent,
        
        children: [
          { path: '', redirectTo: 'Feed', pathMatch: 'full' },
          { path: 'Feed', component: FeedComponent, canActivate:[DashboardGuard], data:{shouldReuse:true,key:'feed'}},
        ],
        
      },
      {
        path:"Profile/:username",
        component:ProfileComponent,
        canActivate:[DashboardGuard]
      },
      {
        path:"Location/:location",
        component:LocationComponent,
        canActivate:[DashboardGuard]
      },
      {
        path:"Search",
        component:SearchComponent,
        canActivate:[DashboardGuard]
      },
      {
        path:"Inbox",
        component:InboxComponent,
        
        
      },
      {
        path:'Chat/:clientID/:chatID',
        component:ChatComponent
      },
      {
        path:'Notifications',
        component:NotificationsComponent
      },
      {
        path:'Post/:postID',
        component:PostViewComponent
      }
      
    ],
    canActivate:[DashboardGuard]
  },
  {
    path:"**", 
    redirectTo:"/Unauthorized"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration:'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
