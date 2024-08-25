import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LandingPageComponent } from './features/components/landing-page/landing-page.component';

import { FeaturesModule } from './features/module/features.module';
import { SharedModule } from './shared/shared.module';



@NgModule({
  declarations: [
    AppComponent,
    
    
    // LandingPageComponent,
    // LoginPageComponent,
    // RegistrationPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FeaturesModule,
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
