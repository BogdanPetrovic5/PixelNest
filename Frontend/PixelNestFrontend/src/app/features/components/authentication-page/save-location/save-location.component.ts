import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { GoogleAuthenticationService } from 'src/app/core/services/authentication/google/google-authentication.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import * as countries from 'i18n-iso-countries';
import en from 'i18n-iso-countries/langs/en.json';
import { CsvReaderService } from 'src/app/core/services/csv/csv-reader.service';
import { UserService } from 'src/app/core/services/user/user.service';
@Component({
  selector: 'app-save-location',
  templateUrl: './save-location.component.html',
  styleUrls: ['./save-location.component.scss']
})
export class SaveLocationComponent {
  locationForm:FormGroup;
  countryList:any;
  filteredCountries:any;
  cityList:any;
  filteredCities:any;
  isSelectableCountry: boolean = false;
  isSelectableCity: boolean = false;

  isSubmitable:boolean = false;
  constructor(
    private _googleAuth:GoogleAuthenticationService,
    private _userSession:UserSessionService,
    private _router:Router,
     private _formBuilder:FormBuilder,
     private _csvReader:CsvReaderService,
     private _userService:UserService
  ){
    this.locationForm = this._formBuilder.group({
      Country: ['',Validators.required],
      City: [{ value: '', disabled: true },Validators.required]
    })
  }
  ngOnInit(): void {
    const state = this._userSession.getFromLocalStorage("state");
    countries.registerLocale(en);

    const countryNames = countries.getNames('en');

    this.countryList = Object.values(countryNames);
    this.filteredCountries = this.countryList;

    
  }
  selectCountry(country:string) {
    this.locationForm.patchValue({
      Country: country
    })
    this.locationForm.get('City')?.enable();
    this._csvReader.getCities(country).subscribe({
      next:response=>{
        
        this.cityList = response;
        this.filteredCities = response
      }
    })
  }
  filterCountries(event: Event) {
    const value = (event.target as HTMLInputElement).value.toLocaleLowerCase();
    
    if(value == "" || value == null) this.filteredCountries = this.countryList;
    
    this.filteredCountries = this.countryList.filter((country:any) =>
      country.toLowerCase().includes(value)
    );
  }
  openSelectionCountry(){
    this.isSelectableCountry = !this.isSelectableCountry;
  }
   
  selectCity(city: string) {
    if(this.cityList != null || this.cityList.length > 0){
      this.locationForm.patchValue({
        City:city
      })
      this.isSubmitable = true;
    }
  }
   
  filterCities(event: Event) {
    const value = (event.target as HTMLInputElement).value.toLocaleLowerCase();
    
    if(value == "" || value == null) this.filteredCities = this.cityList;
    
    this.filteredCities = this.cityList.filter((city:any) =>
      city.toLowerCase().includes(value)
    );
  }
  openSelectionCity() {
    this.isSelectableCity = !this.isSelectableCity
  }
  submit(){
    if(this.locationForm.valid){
      this._userService.updateLocation(this.locationForm.value).subscribe({
        next:response=>{
          this._router.navigate(["Authentication/Redirect-Page"])
        }
      })
    }
   
  }
}
