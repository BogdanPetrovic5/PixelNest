import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LandingPageComponent } from './features/components/landing-page/landing-page.component';
import { LoginPageComponent } from './features/components/login-page/login-page.component';
import { RegistrationPageComponent } from './features/components/registration-page/registration-page.component';
import { FeaturesModule } from './features/module/features.module';

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
    FeaturesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
