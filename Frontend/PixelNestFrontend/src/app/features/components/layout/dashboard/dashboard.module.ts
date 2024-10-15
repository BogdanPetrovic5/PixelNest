import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';

import { SharedModule } from 'src/app/shared/shared.module';


import { RouterOutlet } from '@angular/router';
import { StoryComponent } from './story/story.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    DashboardComponent,
    StoryComponent
 
    
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterOutlet,
    FormsModule
  ]
  
})
export class DashboardModule { }
