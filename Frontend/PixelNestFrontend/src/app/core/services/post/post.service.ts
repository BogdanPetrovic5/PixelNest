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
    return this._httpClient.post<any>(url, body);
  }

  getPosts(currentPage:number):Observable<any>{
    const url = `${environment.apiUrl}/api/Post/GetPosts?page=${currentPage}`
    return this._httpClient.get<any>(url)
  }

  likePost(postID:number, username:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Post/LikePost`
    return this._httpClient.post(url, {
      PostID:postID,
      Username:username
    })
  }
  
  savePost(username:string, postID:number){
    const url = `${environment.apiUrl}/api/Post/SavePost`
    return this._httpClient.post<any>(url,{
      Username:username,
      PostID:postID
    })
  }

  addComment(commentText:string, username:string, postID:number, parentCommentID?:number):Observable<any>{
    const url = `${environment.apiUrl}/api/Post/Comment`
    return this._httpClient.post(url, {
      CommentText:commentText,
      Username:username,
      PostID:postID,
      ParentCommentID:parentCommentID
    })
  }
}
