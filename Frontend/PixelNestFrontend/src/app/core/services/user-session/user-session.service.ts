import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {

  constructor(private _cookieService:CookieService) { }
  clearCookies(){
    this._cookieService.deleteAll();
  }
  getUsername(){
    return this._cookieService.get("username");
  }

  setUsername(username:string){
    this._cookieService.set("username", username);
  }

}
