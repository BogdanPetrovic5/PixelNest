import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, timer } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { DashboardStateService } from '../states/dashboard-state.service';

@Injectable({
  providedIn: 'root'
})
export class UserSessionService {
  private _logOutDialog = new BehaviorSubject<boolean>(false);
  private _userActivity$ = new BehaviorSubject<boolean>(false);
  private _refreshInterval = 5 * 60 * 1000;

  private _currentUrl = new BehaviorSubject<string>("/Dashboard/Feed")
  currentUrl$ = this._currentUrl.asObservable()

  private _tokenExpiration: number | null = null;
  logOutDialog$ = this._logOutDialog.asObservable();

  constructor(
    private _cookieService:CookieService, 
    private _httpClient:HttpClient,
    private _dashboardState:DashboardStateService
  ) { }
 setUrl(value:string){
  this._currentUrl.next(value)
 }
  setTokenExpiration(expiration: string): void {
    this._tokenExpiration = new Date(expiration).getTime();
    this.scheduleTokenRefresh();
  }
  private scheduleTokenRefresh(): void {
    if (!this._tokenExpiration) return;

    const currentTime = new Date().getTime();
    const timeUntilExpiration = this._tokenExpiration - currentTime;
    const timeBeforeRefresh = timeUntilExpiration - this._refreshInterval;

    if (timeBeforeRefresh > 0) {
      timer(timeBeforeRefresh).subscribe(() => {
        console.log(this._userActivity$.getValue());
        if(this._userActivity$.getValue() == true){
          this.refreshToken();
        }else{
          this._dashboardState.setSessionExpiredDialog(true);
        }
        
      });
    } else {
      console.warn('Token is already expired or too close to expiration!');
    }
  }
  refreshToken(){
    const url = `${environment.apiUrl}/api/Authentication/RefreshToken`
    this._httpClient.post<any>(url,{}).subscribe({
      next:response=>{
        this.setToCookie("tokenExpirationAt", response.tokenExpiration)
        this.setToCookie("email", response.email)
        this.setToCookie("username", response.username)
        this.setToCookie("userID", response.clientGuid)
        this.setTokenExpiration(response.tokenExpiration);
        console.log(response.tokenExpiration)
      }
    })
  }
  monitorUserActivity(): void {
    const events = ['mousemove', 'keydown', 'scroll', 'click'];
    events.forEach(event =>
      window.addEventListener(event, () => this._userActivity$.next(true))
    );

    
    setInterval(() => this._userActivity$.next(false), 60000); 
  }

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
    const expiryDate = new Date();
    expiryDate.setMinutes(expiryDate.getMinutes() + 30);
    this._cookieService.set(key, value,expiryDate, '/');
  }
  deleteKeyFromCookie(key:any){
    this._cookieService.delete(key);
  }
  setToLocalStorage(key:string, value:any){
    localStorage.setItem(key, JSON.stringify(value));
  }
  getFromLocalStorage(key:string){
    let value = localStorage.getItem(key)
    if(value != null) return JSON.parse(value);
    return null;
  }

  setLogOutDialog(value:boolean){
    this._logOutDialog.next(value);
  }

  
  
}
