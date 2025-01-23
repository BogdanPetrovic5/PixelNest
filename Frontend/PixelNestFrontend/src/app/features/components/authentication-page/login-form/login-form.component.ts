import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent {
  loginForm:FormGroup;
  error:boolean = false;
  errorMessage:string = ""
  constructor(
    private _router:Router,
    private _formBuilder:FormBuilder,
    private _authService:AuthenticationService,
    private _lottieState:LottieStateService,
    private _userSession:UserSessionService
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
    if(!this.loginForm.hasError('required')){
      this._lottieState.setIsInitialized(true);
      this._authService.login(loginFormValues).subscribe((response) =>{
        this._lottieState.setIsInitialized(false);
        this._lottieState.setIsSuccess(true)
        setTimeout(() => {
          
          this._lottieState.setIsSuccess(false)
          this._router.navigate(["/Dashboard"])
          this._userSession.setToCookie("tokenExpirationAt", response.tokenExpiration)
          console.log(response.tokenExpiration)
        }, 1500);
       
      },(error:HttpErrorResponse)=>{
        this._lottieState.setIsInitialized(false);  
        this.errorMessage = "";
        setTimeout(() => {
            this.error = true;
            this.errorMessage = error.error?.response || "An unexpected error occurred.";
        }, 0);
        this.loginForm.reset({
            Email: '',
            Password: ''
        });
        setTimeout(()=>{
            this.error = false;    
        }, 2000)
      }
    
      )
    }
    
  }
}
