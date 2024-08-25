import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './features/components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from './features/components/authentication-page/authentication-page.component';
import { LoginFormComponent } from './features/components/authentication-page/login-form/login-form.component';
import { RegisterFormComponent } from './features/components/authentication-page/register-form/register-form.component';


const routes: Routes = [
  {path:'',redirectTo:"/Get Started", pathMatch:'full'},
  {path:"Get Started", component:LandingPageComponent},
  {
    path:"Authentication", 
    component:AuthenticationPageComponent,
    children: [
      { path: '', redirectTo: 'Register', pathMatch: 'full' },
      { path: 'Register', component: RegisterFormComponent },
      { path: 'Login', component: LoginFormComponent }
    ]
  },

  {path:"**", redirectTo:"/Get Started"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
