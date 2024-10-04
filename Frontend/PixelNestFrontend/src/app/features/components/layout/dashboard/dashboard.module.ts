import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { FeedComponent } from './feed/feed.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NewPostComponent } from './new-post/new-post.component';

import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    DashboardComponent,
    FeedComponent,
    NewPostComponent,
    
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterOutlet,
    FormsModule
  ]
  
})
export class DashboardModule { }
