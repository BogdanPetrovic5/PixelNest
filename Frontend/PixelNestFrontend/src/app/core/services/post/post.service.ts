import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private _httpClient:HttpClient) { }

  createNewPost(body:any):Observable<any>{
   
    const url = `${environment.apiUrl}/api/Post/ShareNewPost`
    return this._httpClient.post<any>(url, body, {withCredentials:true});
  }
}
