import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { FeedComponent } from './feed/feed.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NewPostComponent } from './new-post/new-post.component';



@NgModule({
  declarations: [
    DashboardComponent,
    FeedComponent,
    NewPostComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
   
  ]
  
})
export class DashboardModule { }