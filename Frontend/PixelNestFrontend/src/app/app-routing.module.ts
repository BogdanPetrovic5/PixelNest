import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './features/components/landing-page/landing-page.component';

const routes: Routes = [
  {path:'', component:LandingPageComponent, pathMatch:'full'},
  {path:"Get Started", component:LandingPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
