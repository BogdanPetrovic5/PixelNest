import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterFormComponent } from './components/register-form/register-form.component';



@NgModule({
  declarations: [
    RegisterFormComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    RegisterFormComponent
  ]
})
export class SharedModule { }
