import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';

import { SharedModule } from 'src/app/shared/shared.module';


import { RouterOutlet } from '@angular/router';
import { StoryComponent } from './story/story.component';
import { FormsModule } from '@angular/forms';
import { NewStoryComponent } from './story/new-story/new-story.component';

import { StoryListComponent } from './story/story-list/story-list.component';
import { StoryPreviewComponent } from './story/story-list/story-preview/story-preview.component';
import { HammerModule } from '@angular/platform-browser';
import { ViewerListComponent } from './story/story-list/story-preview/viewer-list/viewer-list.component';
import { DashboardRoutingModule } from './dashboard-routing.module';


@NgModule({
  declarations: [
    DashboardComponent,
    StoryComponent,
    NewStoryComponent,
  
    StoryListComponent,
    StoryPreviewComponent,
    ViewerListComponent,
    
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    SharedModule,
    RouterOutlet,
    FormsModule,
    HammerModule,
    
  ]
  
})
export class DashboardModule { }
