import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class GoogleAuthenticationService {

  constructor(private _httpClient:HttpClient) {

  }
  getGoogleResponse(state:string){
    const url = `${environment.apiUrl}/api/Authentication/GetLoginResponse?state=${state}` 
    return this._httpClient.get<any>(url);
  }
  loginWithGoogle(state:string){
      const url = `${environment.apiUrl}/api/Authentication/SaveState?state=${state}`
      return this._httpClient.post<any>(url,{});
  }
}
