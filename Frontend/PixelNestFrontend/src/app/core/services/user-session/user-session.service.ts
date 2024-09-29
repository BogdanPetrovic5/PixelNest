import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {

  constructor(private _cookieService:CookieService) { }

  clearCookies(){
    this._cookieService.deleteAll('/');
  }

  getFromCookie(key:string){
    return this._cookieService.get(key);
  }
  clearStorage(){
    localStorage.clear()
  }
  setToCookie(key:any, value:any){
    this._cookieService.set(key, value, {path:"/"});
  }

  setToLocalStorage(key:string, value:any){
    localStorage.setItem(key, JSON.stringify(value));
  }
  getFromLocalStorage(key:string){
    let value = localStorage.getItem(key)
    if(value != null) return JSON.parse(value);
    return null;
  }
}
