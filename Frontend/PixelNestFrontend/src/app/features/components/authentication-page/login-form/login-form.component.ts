import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent {
  loginForm:FormGroup;
  constructor(
    private _router:Router,
    private _formBuilder:FormBuilder,
    private _authService:AuthenticationService,
    private _authState:AuthStateService
  ){
    this.loginForm = this._formBuilder.group({
      Email:['', Validators.required],
      Password:['', Validators.required]
    })
  }
  navigateToRegister(){
    this._router.navigate(['/Authentication/Register'])
  }
  

  login(){
    const loginFormValues = this.loginForm.value;
    this._authService.login(loginFormValues).subscribe((response) =>{
      this._authState.setIsAuthSuccess(true)
      setTimeout(() => {
        this._authState.setIsAuthSuccess(false)
        this._router.navigate(["/Dashboard"])
      }, 1500);
     
    },(error:HttpErrorResponse)=>{
        console.log(error)
    }
  
    )
  }
}
