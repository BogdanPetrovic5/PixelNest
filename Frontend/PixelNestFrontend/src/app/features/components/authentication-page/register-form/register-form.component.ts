import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss']
})
export class RegisterFormComponent {
  registerForm:FormGroup;
  constructor(
    private _router:Router,
    private _formBuilder:FormBuilder,
    private _authService:AuthenticationService
  ){
    this.registerForm = this._formBuilder.group({
      Firstname: ['', Validators.required],
      Lastname: ['', Validators.required],
      Username: ['', Validators.required],
      Password: ['', [Validators.required, Validators.minLength(6)]],
      Email: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
      
    })
  }
  navigateToLogin(){
    this._router.navigate(['/Authentication/Login'])
  }

  hasEmptyFields(){
    let hasEmpty = false;

    Object.keys(this.registerForm.controls).forEach(key =>{
      const control = this.registerForm.get(key);

      if (control && (control.value === '' || control.value === null || control.value === undefined)) {
        hasEmpty = true;
      }
    })
  }



  register(){
    const formValues = this.registerForm.value;
    this._authService.register(formValues).subscribe((response)=>{
      console.log(response.message)
    },(error:HttpErrorResponse) =>{
      console.log(error);
    })
  }
}