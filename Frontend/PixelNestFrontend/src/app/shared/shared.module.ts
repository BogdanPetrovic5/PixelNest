import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterFormComponent } from '../features/components/authentication-page/register-form/register-form.component';
import { LoginFormComponent } from '../features/components/authentication-page/login-form/login-form.component';
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
    LottieLoadingComponent
  ]
})
export class SharedModule { }
