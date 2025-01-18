import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BannerComponent } from './components/banner/banner.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { PostComponent } from './components/post/post.component';
import { AlertComponent } from './components/alert/alert.component';
import { LottieSuccessComponent } from './components/lottie-success/lottie-success.component';
import { LottieFailComponent } from './components/lottie-fail/lottie-fail.component';
import { LikesComponent } from './components/post/likes/likes.component';
import { CommentsComponent } from './components/post/comments/comments.component';
import { FormsModule } from '@angular/forms';
import { CommentComponent } from './components/post/comments/comment/comment.component';
import { ReplyComponent } from './components/post/comments/comment/reply/reply.component';
import { LikeCommentComponent } from './components/post/comments/like-comment/like-comment.component';
import { LottieLoadingComponent } from './components/lottie-loading/lottie-loading.component';
import { LocationComponent } from './components/location/location.component';
import { FeedComponent } from './components/feed/feed.component';
import { DeleteDialogComponent } from './components/post/delete-dialog/delete-dialog.component';

import { UserTagComponent } from './components/user-tag/user-tag.component';
import { ProfileImageComponent } from './components/profile-image/profile-image.component';
import { LogOutDialogComponent } from './components/log-out-dialog/log-out-dialog.component';
import { TopNavigationComponent } from './components/top-navigation/top-navigation.component';
import { ChatComponent } from './components/chat/chat.component';
import { MessageComponent } from './components/message/message.component';
import { TimeAgoPipe } from './pipes/time-ago.pipe';
import { LottieNotificationComponent } from './components/lottie-notification/lottie-notification.component';
import { InboxIconComponent } from './components/inbox-icon/inbox-icon.component';

@NgModule({
  declarations: [
    BannerComponent,
    NavigationComponent,
    PostComponent,
    AlertComponent,
    LottieSuccessComponent,
    LottieFailComponent,
    LottieLoadingComponent,
    LikesComponent,
    CommentsComponent,
    CommentComponent,
    ReplyComponent,
    LikeCommentComponent,
    LocationComponent,
    FeedComponent,
    DeleteDialogComponent,
    UserTagComponent,
    ProfileImageComponent,
    LogOutDialogComponent,
    TopNavigationComponent,
    ChatComponent,
    MessageComponent,
    TimeAgoPipe,
    LottieNotificationComponent,
    InboxIconComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  schemas: [
    NO_ERRORS_SCHEMA,
    CUSTOM_ELEMENTS_SCHEMA
    
  ],
  exports: [
    NavigationComponent,
    PostComponent,
    AlertComponent,
    LottieSuccessComponent,
    LottieFailComponent,
    LottieLoadingComponent,
    FeedComponent,
    DeleteDialogComponent,
    UserTagComponent,
    ProfileImageComponent,
    LogOutDialogComponent,
    TopNavigationComponent,
    ChatComponent,
    MessageComponent,
    TimeAgoPipe,
    LottieNotificationComponent,
    InboxIconComponent
  ]
})
export class SharedModule { }
