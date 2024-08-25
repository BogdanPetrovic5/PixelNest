import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './features/components/landing-page/landing-page.component';
import { AuthenticationPageComponent } from './features/components/authentication-page/authentication-page.component';


const routes: Routes = [
  {path:'',redirectTo:"/Get Started", pathMatch:'full'},
  {path:"Get Started", component:LandingPageComponent},
  {path:"Authentication", component:AuthenticationPageComponent},
  {path:"**", component:LandingPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
