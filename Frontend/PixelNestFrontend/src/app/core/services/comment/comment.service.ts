import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private _httpClient:HttpClient) { }

  getComments(postID: number):Observable<any>{
    const url = `${environment.apiUrl}/api/Comment/GetComments?postID=${postID}`
    return this._httpClient.get<any>(url);

  }
  getReplies(initialParentID?:number){
    const url = `${environment.apiUrl}/api/Comment/GetReplies?initialParentID=${initialParentID}`
    return this._httpClient.get<any>(url);
  }
}
