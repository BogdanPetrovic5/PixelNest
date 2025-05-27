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
    const url = `${environment.apiUrl}/api/comment/comments?postID=${postID}`
    return this._httpClient.get<any>(url);

  }

  getReplies(initialParentID?:number):Observable<any>{
    const url = `${environment.apiUrl}/api/comment/replies?initialParentID=${initialParentID}`
    return this._httpClient.get<any>(url);
  }

  likeComment(commentID?:number, username?:string){
    const url = `${environment.apiUrl}/api/${commentID}/like`;

    return this._httpClient.post<any>(url, {

      Username:username,
      CommentID:commentID
    })
  }
}
