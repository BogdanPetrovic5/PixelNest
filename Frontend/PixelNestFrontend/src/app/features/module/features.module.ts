import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LandingPageComponent } from '../components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from '../components/authentication-page/authentication-page.component';
import { SharedModule } from 'src/app/shared/shared.module';



@NgModule({
  declarations: [
    LandingPageComponent,
    AuthenticationPageComponent,
  
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
    
  ]
})
export class FeaturesModule { }
