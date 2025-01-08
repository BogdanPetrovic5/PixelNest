import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardModule } from './dashboard/dashboard.module';
import { NewPostComponent } from './new-post/new-post.component';
import { ProfileModule } from './profile/profile.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { LayoutComponent } from './layout.component';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { SearchComponent } from './search/search.component';
@NgModule({
  declarations: [
    NewPostComponent,
    LayoutComponent,
    SearchComponent
  ],
  imports: [
    CommonModule,
    ProfileModule,
    DashboardModule,
    SharedModule,
    FormsModule,
    AppRoutingModule
  ]
})
export class LayoutModule { }
