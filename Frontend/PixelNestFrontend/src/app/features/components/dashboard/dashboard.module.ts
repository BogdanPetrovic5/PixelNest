import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { FeedComponent } from './feed/feed.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NewPostComponent } from './new-post/new-post.component';
import { ProfileComponent } from './profile/profile.component';
import { RouterOutlet } from '@angular/router';


@NgModule({
  declarations: [
    DashboardComponent,
    FeedComponent,
    NewPostComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterOutlet
  ]
  
})
export class DashboardModule { }
