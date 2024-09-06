import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { HttpHeaders} from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment.development';
import { Observable, tap } from 'rxjs';
import { UserSessionService } from '../user-session/user-session.service';
@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private _httpClient:HttpClient,
    private _userSessionService:UserSessionService
  ) { }
  
  register(formGroup:any):Observable<any>{
    const url = `${environment.apiUrl}/api/Authentication/Register`;
    return this._httpClient.post<any>(url, formGroup)
  }

  login(formGroup:any):Observable<any>{
    const url = `${environment.apiUrl}/api/Authentication/Login`;
    return this._httpClient.post<any>(url, formGroup).pipe(
      tap(response=> this._storeUsername(response.username))
    )
  }
  
  private _storeUsername(username:string){
    console.log(username)
    this._userSessionService.setUsername(username)
  }
}
