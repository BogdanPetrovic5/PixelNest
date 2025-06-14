import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from './components/authentication-page/authentication-page.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoginFormComponent } from './components/authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './components/authentication-page/register-form/register-form.component';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { LayoutModule } from './components/layout/layout.module';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';
import { RedirectPageComponent } from './components/authentication-page/redirect-page/redirect-page.component';
import { SaveLocationComponent } from './components/authentication-page/save-location/save-location.component';






@NgModule({
  declarations: [
    LandingPageComponent,
    AuthenticationPageComponent,
    LoginFormComponent,
    RegisterFormComponent,
    UnauthorizedComponent,
    RedirectPageComponent,
    SaveLocationComponent,
  ],
  imports: [
    CommonModule,
    LayoutModule,
    SharedModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule
    
  ],
  schemas: [
  
  ],
  exports:[
  
  ],
  providers:[
   
  ]
})
export class FeaturesModule { }
