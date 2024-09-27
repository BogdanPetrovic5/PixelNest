import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _httpClient:HttpClient) { }
  getUserData(username:string):Observable<any>{
    const url = `${environment.apiUrl}/api/User/GetUserProfile?username=${username}`
    return this._httpClient.get(url);
  }
}
