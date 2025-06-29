import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { PostDto } from '../../dto/post.dto';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private _httpClient:HttpClient) { }
  getSinglePost(postID:string):Observable<PostDto>{
    const url = `${environment.apiUrl}/api/post/${postID}`
    return this._httpClient.get<PostDto>(url);
  }
  createNewPost(body:any):Observable<any>{
    const url = `${environment.apiUrl}/api/post/new`
    return this._httpClient.post<any>(url, body);
  }

  getPosts(currentPage:number, parameter?:string ):Observable<any>{
    let url = `${environment.apiUrl}/api/post/posts?page=${currentPage}`;
    if (parameter) {
      url = `${environment.apiUrl}/api/post/posts?${parameter}&page=${currentPage}`;
    }
  return this._httpClient.get<any>(url);
    
  }
  likePost(postGuid?:string):Observable<any>{
    const url = `${environment.apiUrl}/api/post/${postGuid}/like`
    return this._httpClient.post(url,{})
  }
  deletePost(postID:string):Observable<any>{
    const url = `${environment.apiUrl}/api/post/${postID}`;
    return this._httpClient.delete<any>(url)
  }
  savePost(postID:string){
    const url = `${environment.apiUrl}/api/post/${postID}/save`
    return this._httpClient.post<any>(url,{
    })
  }

  addComment(commentText:string, username:string, postID:string, parentCommentID?:number):Observable<any>{
    const url = `${environment.apiUrl}/api/post/comment`
    return this._httpClient.post(url, {
      CommentText:commentText,
      Username:username,
      PostID:postID,
      ParentCommentID:parentCommentID
    })
  }
  
}
