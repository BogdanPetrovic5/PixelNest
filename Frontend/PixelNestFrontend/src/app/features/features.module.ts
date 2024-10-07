import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from './components/authentication-page/authentication-page.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoginFormComponent } from './components/authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './components/authentication-page/register-form/register-form.component';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DashboardModule } from './components/layout/dashboard/dashboard.module';
import { ProfileComponent } from './components/layout/profile/profile.component';
import { FollowingsComponent } from './components/layout/profile/followings/followings.component';
import { FollowersComponent } from './components/layout/profile/followers/followers.component';
import { LayoutComponent } from './components/layout/layout.component';
import { NewPostComponent } from './components/layout/new-post/new-post.component';



@NgModule({
  declarations: [
    LandingPageComponent,
    AuthenticationPageComponent,
    LoginFormComponent,
    RegisterFormComponent,
    ProfileComponent,
    FollowingsComponent,
    FollowersComponent,
    LayoutComponent,
    NewPostComponent
    
  ],
  imports: [
    CommonModule,
    SharedModule,
    AppRoutingModule,
    ReactiveFormsModule,
    DashboardModule,
    FormsModule
    
  ],
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
    
  ],
  exports:[
  
  ]
})
export class FeaturesModule { }
