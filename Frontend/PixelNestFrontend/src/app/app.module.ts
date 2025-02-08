import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule,HammerModule, HAMMER_GESTURE_CONFIG  } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FeaturesModule } from './features/features.module';
import { SharedModule } from './shared/shared.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { GlobalInterceptor } from './core/interceptors/global.interceptor';
import { CustomHammerConfig } from './hammer-config';
import { UnauthorizedAccessInterceptor } from './core/interceptors/unauthorized-access.interceptor';
import { RouteReuseStrategy } from '@angular/router';
import { CustomRouteReuseStrategy } from './core/route-reuse-strategy';
import { ApiTrackerInterceptor } from './core/interceptors/api-tracker.interceptor';


@NgModule({
  declarations: [
    AppComponent,
   
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FeaturesModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [
    {
      provide:RouteReuseStrategy,
      useClass:CustomRouteReuseStrategy
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: GlobalInterceptor,
      multi: true
    },
    {
      provide:HTTP_INTERCEPTORS,
      useClass: UnauthorizedAccessInterceptor,
      multi:true
    },
    {
      provide:HTTP_INTERCEPTORS,
      useClass:ApiTrackerInterceptor,
      multi:true
    },
    { 
      provide: HAMMER_GESTURE_CONFIG, 
      useClass: CustomHammerConfig 
    },
   
  ],
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
