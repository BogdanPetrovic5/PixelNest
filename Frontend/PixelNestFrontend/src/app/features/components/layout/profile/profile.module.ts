import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FollowersComponent } from './followers/followers.component';
import { FollowingsComponent } from './followings/followings.component';
import { ProfileComponent } from './profile.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { EditComponent } from './edit/edit.component';
import { FormsModule } from '@angular/forms';

import { BrowserModule } from '@angular/platform-browser';
import { ImageCropperModule } from 'ngx-image-cropper';


@NgModule({
  declarations: [
    FollowersComponent,
    FollowingsComponent,
    ProfileComponent,
    EditComponent,
   
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
   
    BrowserModule,
    ImageCropperModule,
  ]
})
export class ProfileModule { }
