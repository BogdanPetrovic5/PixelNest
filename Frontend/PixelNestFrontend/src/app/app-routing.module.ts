import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './features/components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from './features/components/authentication-page/authentication-page.component';
import { LoginFormComponent } from './features/components/authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './features/components/authentication-page/register-form/register-form.component';
import { FeedComponent } from './features/components/dashboard/feed/feed.component';
import { DashboardComponent } from './features/components/dashboard/dashboard.component';
import { AuthGuard } from './core/guards/auth.guard';
import { DashboardGuard } from './core/guards/dashboard.guard';
import { ProfileComponent } from './features/components/profile/profile.component';


const routes: Routes = [
  {path:'',redirectTo:"Dashboard", pathMatch:'full'},
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
  {path:"**", redirectTo:"/Get Started"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
