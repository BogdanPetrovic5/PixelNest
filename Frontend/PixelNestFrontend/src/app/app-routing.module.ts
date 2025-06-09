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
import { RedirectPageComponent } from './features/components/authentication-page/redirect-page/redirect-page.component';
import { AnalyticsComponent } from './features/components/layout/profile/analytics/analytics.component';
import { SaveLocationComponent } from './features/components/authentication-page/save-location/save-location.component';


const routes: Routes = [
  {path:'',redirectTo:"unauthorized", pathMatch:'full'},
  {path:"unauthorized", component:UnauthorizedComponent},
  {path:"get started", component:LandingPageComponent},
  {
    path:"authentication", 
    component:AuthenticationPageComponent,
    children: [
      { path: '', redirectTo: 'register', pathMatch: 'full' },
      { path: 'register', component: RegisterFormComponent,canActivate:[AuthGuard] },
      { path: 'login', component: LoginFormComponent,canActivate:[AuthGuard] },
      { path: 'redirect-page', component:RedirectPageComponent},
      { path:'save-location', component:SaveLocationComponent}
    ],
    
  },
  {
    path:"",
    component:LayoutComponent,
    children:[
      {
       path: 'dashboard',
      loadChildren: () => import('./features/components/layout/dashboard/dashboard.module').then(m => m.DashboardModule),
      
        
      },
      {
        path:"profile/:username",
        component:ProfileComponent,
        canActivate:[DashboardGuard],
      },
      {
        path:"profile/:username/analytics",
        component:AnalyticsComponent,
        canActivate:[DashboardGuard],
      },
      {
        path:"location/:location",
        component:LocationComponent,
        canActivate:[DashboardGuard]
      },
      {
        path:"search",
        component:SearchComponent,
        canActivate:[DashboardGuard]
      },
      {
        path:"inbox",
        component:InboxComponent,
        
        
      },
      {
        path:'chat/:clientID/:chatID',
        component:ChatComponent
      },
      {
        path:'notifications',
        component:NotificationsComponent
      },
      {
        path:'post/:postID',
        component:PostViewComponent
      }
      
    ],
    canActivate:[DashboardGuard]
  },
  {
    path:"**", 
    redirectTo:"/unauthorized"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration:'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
