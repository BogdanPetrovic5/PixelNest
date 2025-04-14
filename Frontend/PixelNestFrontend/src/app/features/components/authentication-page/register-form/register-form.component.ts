import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/services/authentication/authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthStateService } from 'src/app/core/services/states/auth-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import * as countries from 'i18n-iso-countries';
import en from 'i18n-iso-countries/langs/en.json';
import { CsvReaderService } from 'src/app/core/services/csv/csv-reader.service';
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
  marginLeft:number = 0;
  barMargin:number = 0;
  marginStep:number = 105;
  errorMessage:string = "";
  navigationText:string = this.pivot ? "< Previous" : "Next >"
  isSelectableCountry:boolean = false;
  isSelectableCity:boolean = false;
  countryList:any;
  filteredCountries:any;
  cityList:any;
  filteredCities:any;
  isDisabled:boolean = true;
  constructor(
    private _router:Router,
    private _formBuilder:FormBuilder,
    private _authService:AuthenticationService,
    private _lottieState:LottieStateService,
    private _csvReader:CsvReaderService
  ){
    this.registerForm = this._formBuilder.group({
      Firstname: ['', Validators.required],
      Lastname: ['', Validators.required],
      Username: ['', Validators.required],
      Password: ['', [Validators.required, Validators.pattern(/^.{6,}$/)]],
      Email: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[email]+\.[a-zA-Z]{2,}$/)]],
      Country: ['',Validators.required],
      City: [{ value: '', disabled: this.isDisabled },Validators.required]
    })
  }
  ngOnInit():void{
    countries.registerLocale(en);

    const countryNames = countries.getNames('en');

    this.countryList = Object.values(countryNames);
    this.filteredCountries = this.countryList;
    

  }
  ngDoCheck():void{
    this.navigationText = this.pivot ? "< Previous" : "Next >"
  }
  filterCountries(event:Event){
    const value = (event.target as HTMLInputElement).value.toLocaleLowerCase();
    
    if(value == "" || value == null) this.filteredCountries = this.countryList;
    
    this.filteredCountries = this.countryList.filter((country:any) =>
      country.toLowerCase().includes(value)
    );
  }
  filterCities(event:Event){
    const value = (event.target as HTMLInputElement).value.toLocaleLowerCase();
    
    if(value == "" || value == null) this.filteredCities = this.cityList;
    
    this.filteredCities = this.cityList.filter((city:any) =>
      city.toLowerCase().includes(value)
    );
  }
  openSelectionCountry(){
    this.isSelectableCountry = !this.isSelectableCountry;
  }
  openSelectionCity(){
    this.isSelectableCity = !this.isSelectableCity
    
  }
  selectCountry(country:string){
    this.registerForm.patchValue({
      Country: country
    })
    this.registerForm.get('City')?.enable();
    this._csvReader.getCities(country).subscribe({
      next:response=>{
        
        this.cityList = response;
        this.filteredCities = response
      }
    })
  }

  togglePivot(){
    if(!this.pivot) this.marginLeft -= 105;
    if(this.pivot) this.marginLeft += 105;
    this.pivot = !this.pivot

  }
  next(){
    if(this.marginStep >= Math.abs(this.marginLeft)){
      this.marginLeft -= this.marginStep;
      this.barMargin += 33
    }
   
    

  }
  previous(){
    if(this.marginStep <= Math.abs(this.marginLeft) ){
      this.marginLeft += this.marginStep;
      this.barMargin -= 33;
    }
   
   
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
