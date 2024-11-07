import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';

import { SharedModule } from 'src/app/shared/shared.module';


import { RouterOutlet } from '@angular/router';
import { StoryComponent } from './story/story.component';
import { FormsModule } from '@angular/forms';
import { NewStoryComponent } from './story/new-story/new-story.component';
import { StoryPreviewComponent } from './story/story-list/story-preview/story-preview.component';
import { StoryListComponent } from './story/story-list/story-list.component';



@NgModule({
  declarations: [
    DashboardComponent,
    StoryComponent,
    NewStoryComponent,
    StoryPreviewComponent,
    StoryListComponent
 
    
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterOutlet,
    FormsModule
  ]
  
})
export class DashboardModule { }
