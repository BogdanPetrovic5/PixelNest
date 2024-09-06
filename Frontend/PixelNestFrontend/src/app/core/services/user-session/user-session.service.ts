import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {

  constructor(private _cookieService:CookieService) { }
  setUsername(username:string){
    this._cookieService.set("username", username);
  }

}
