import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NewPostComponent } from './new-post/new-post.component';
import { ProfileModule } from './profile/profile.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { LayoutComponent } from './layout.component';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { SearchComponent } from './search/search.component';
import { InboxComponent } from './inbox/inbox.component';
import { NotificationComponent } from './notification-modal/notification.component';
import { SessionExpiredDialogComponent } from './session-expired-dialog/session-expired-dialog.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { PostViewComponent } from './post-view/post-view.component';


@NgModule({
  declarations: [
    NewPostComponent,
    LayoutComponent,
    SearchComponent,
    InboxComponent,
    NotificationComponent,
    SessionExpiredDialogComponent,
    NotificationsComponent,
    PostViewComponent
   
  ],
  imports: [
    CommonModule,
    ProfileModule,
   
    SharedModule,
    FormsModule,
    AppRoutingModule,
   
  ],
  providers:[
  
  ],
 
})
export class LayoutModule { }
