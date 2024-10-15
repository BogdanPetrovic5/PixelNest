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


const routes: Routes = [
  {path:'',redirectTo:"Get Started", pathMatch:'full'},
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
          { path: 'Feed', component: FeedComponent, canActivate:[DashboardGuard] },
        ],
        canActivate:[DashboardGuard]
      },
      {
        path:"Profile/:username",
        component:ProfileComponent,
        
      },
      {
        path:"Location/:location",
        component:LocationComponent
      },
    ]
  },
  {
    path:"**", 
    redirectTo:"/Get Started"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
