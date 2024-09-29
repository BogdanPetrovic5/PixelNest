import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse} from '@angular/common/http';
import { HttpHeaders} from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment.development';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { UserSessionService } from '../user-session/user-session.service';
@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private _httpClient:HttpClient,
    private _userSessionService:UserSessionService
  ) { }
  ngOnInit():void{
    
  }
  logout(email:string):Observable<any>{
    let url = `${environment.apiUrl}/api/Authentication/Logout`
    return this._httpClient.post<any>(url, {
      Email:email
    }, {withCredentials:true});
  }
  register(formGroup:any):Observable<any>{
    const url = `${environment.apiUrl}/api/Authentication/Register`;
    return this._httpClient.post<any>(url, formGroup)
  }

  login(formGroup:any):Observable<any>{
    const url = `${environment.apiUrl}/api/Authentication/Login`;
    return this._httpClient.post<any>(url, formGroup, {withCredentials:true}).pipe(
      tap(response=> {
        this._storeCredentials(response)
        this.isLoggedIn()
      })
    )
  }

  isLoggedIn():Observable<any>{
    return this._httpClient.post<{loggedIn:boolean}>(`${environment.apiUrl}/api/Authentication/IsLoggedIn`,{},{withCredentials:true})
    .pipe(
      map((response:any) => response.loggedIn)
      ,
      catchError((error:HttpErrorResponse)=>{
        return of(false)
      })
    )
  }
  private _storeCredentials(response:any){
 
    this._userSessionService.setToCookie("username",response.username)
    this._userSessionService.setToCookie("email", response.email);
  }
}
