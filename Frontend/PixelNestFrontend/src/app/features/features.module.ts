import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from './components/authentication-page/authentication-page.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoginFormComponent } from './components/authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './components/authentication-page/register-form/register-form.component';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DashboardModule } from './components/dashboard/dashboard.module';



@NgModule({
  declarations: [
    LandingPageComponent,
    AuthenticationPageComponent,
    LoginFormComponent,
    RegisterFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AppRoutingModule,
    ReactiveFormsModule,
    DashboardModule
    
  ],
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
    
  ],
  exports:[
  
  ]
})
export class FeaturesModule { }