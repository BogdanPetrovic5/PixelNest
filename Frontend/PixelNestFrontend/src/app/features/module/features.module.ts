import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingPageComponent } from '../components/landing-page/landing-page.component';
import { LoginPageComponent } from '../components/login-page/login-page.component';
import { RegistrationPageComponent } from '../components/registration-page/registration-page.component';



@NgModule({
  declarations: [
    LandingPageComponent,
     LoginPageComponent, 
     RegistrationPageComponent
  ],
  imports: [
    CommonModule
  ]
})
export class FeaturesModule { }
