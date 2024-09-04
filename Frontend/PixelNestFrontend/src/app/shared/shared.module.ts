import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterFormComponent } from '../features/components/authentication-page/register-form/register-form.component';
import { LoginFormComponent } from '../features/components/authentication-page/login-form/login-form.component';
import { BannerComponent } from './components/banner/banner.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { PostComponent } from './components/post/post.component';

@NgModule({
  declarations: [
    BannerComponent,
    NavigationComponent,
    PostComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    NavigationComponent,
    PostComponent,
    

  ]
})
export class SharedModule { }
