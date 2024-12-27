import { Component, OnInit } from '@angular/core';
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
export class RegisterFormComponent implements OnInit{
  registerForm:FormGroup;

  error:boolean = false
  enabled:boolean = false;
  pivot?:boolean = false;

  errorMessage:string = "";
  navigationText:string = this.pivot ? "< Previous" : "Next >"
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
  ngOnInit():void{
  

  }
  ngDoCheck():void{
    this.navigationText = this.pivot ? "< Previous" : "Next >"
  }
  togglePivot(){
    this.pivot = !this.pivot
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
      this.enabled = true;
      this._lottieState.setIsInitialized(true);
      this._authService.register(formValues).subscribe({
        next: (response) => {
          this.registerForm.reset({
            Firstname:'',
            Lastname:'',
            Email:'',
            Username:'',
            Password:'',

          })
          this._lottieState.setIsInitialized(false);
          this._lottieState.setIsSuccess(true);
         
          setTimeout(() => {
            this._lottieState.setIsSuccess(false);
            this.enabled = true;
            this.navigateToLogin();
          }, 1500);
        },
        error: (error: HttpErrorResponse) => {
          this._lottieState.setIsInitialized(false);
          this.error = true;
          this.enabled = true;
          this.errorMessage = error.error?.message || "An unexpected error occurred.";
          setTimeout(() => {
            this.error = false;
          }, 2000);
        }
      });
    }else this.registerForm.markAllAsTouched()
    
  }
}
