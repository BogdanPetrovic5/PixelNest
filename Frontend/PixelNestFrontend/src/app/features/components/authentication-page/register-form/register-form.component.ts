import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss']
})
export class RegisterFormComponent {
  registerForm:FormGroup;

  enabled:boolean = false;
  constructor(
    private _router:Router,
    private _formBuilder:FormBuilder,
    private _authService:AuthenticationService,
    private _lottieState:LottieStateService
  ){
    this.registerForm = this._formBuilder.group({
      Firstname: ['', Validators.required],
      Lastname: ['', Validators.required],
      Username: ['', Validators.required],
      Password: ['', [Validators.required, Validators.pattern(/^.{6,}$/)]],
      Email: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[email]+\.[a-zA-Z]{2,}$/)]],
      
    })
  }
  ngDoCheck():void{
    if(this.registerForm.valid){
      this.enabled = true;
    }else this.enabled = false;
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
    if(this.registerForm.valid){
      this._authService.register(formValues).subscribe((response)=>{
        this._lottieState.setIsSuccess(true)
        setTimeout(() => {
          this._lottieState.setIsSuccess(false)
          this.navigateToLogin();
        }, 1500);
      },(error:HttpErrorResponse) =>{
        console.log(error);
      })
    }else this.registerForm.markAllAsTouched()
    
  }
}
