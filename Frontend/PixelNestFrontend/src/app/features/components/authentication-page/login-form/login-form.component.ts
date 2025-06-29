import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { GoogleAuthenticationService } from 'src/app/core/services/authentication/google/google-authentication.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})

export class LoginFormComponent implements OnInit{
  loginForm:FormGroup;
  error:boolean = false;
  errorMessage:string = ""
 
  constructor(
    private _router:Router,
    private _formBuilder:FormBuilder,
    private _authService:AuthenticationService,
    private _lottieState:LottieStateService,
    private _userSession:UserSessionService,
    private _googleAuth:GoogleAuthenticationService
  ){
    this.loginForm = this._formBuilder.group({
      Email:['', Validators.required],
      Password:['', Validators.required]
    })
  }
  navigateToRegister(){
    this._router.navigate(['/authentication/register'])
  }

  ngOnInit():void{
    
  }

 signWithGoogle(){
    const state = this._generateRandomString(16);
    this._userSession.setToLocalStorage("state", state);
    this._googleAuth.loginWithGoogle(state).subscribe({
      next:response=>{
        window.location.href = 
          `https://accounts.google.com/o/oauth2/auth?` +
          `client_id=928125021910-7risb5280cb3776v26paipadj1vfj9gf.apps.googleusercontent.com` +
          `&redirect_uri=http://localhost:7157/api/authentication/google/signin` +
          `&response_type=code` +  
          `&scope=openid email profile` +
          `&access_type=offline` + 
          `&state=${state}`,
          '_blank'
        
      }
    })
    

    
 } 



  login(){
      if(!this.loginForm.valid) return

      const loginFormValue = this.loginForm.value;

      this._lottieState.setIsInitialized(true);
      this._authService.login(loginFormValue).pipe(
        finalize(()=>{
          this._lottieState.setIsInitialized(false);
        })
      ).subscribe({
        next:response=>{
          this._lottieState.setIsSuccess(true);

          setTimeout(() => {
            this._lottieState.setIsSuccess(false);
            this._router.navigate(['/dashboard']);
            this._userSession.setToCookie('tokenExpirationAt', response.tokenExpiration);
          }, 1500);
        },
        error:error=>{
          this._handleError(error)
        }
      })
      
  }
  private _handleError(error: any) {
    this.errorMessage = '';

    this.loginForm.reset({
      Email: '',
      Password: ''
    });

    setTimeout(() => {
      this.error = true;
      this.errorMessage = error.error?.response || 'An unexpected error occurred.';
    }, 0);

    setTimeout(() => {
      this.error = false;
    }, 2000);
  }
  
  private _generateRandomString(length: number): string {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    const charactersLength = characters.length;
    for (let i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
  } 
}
