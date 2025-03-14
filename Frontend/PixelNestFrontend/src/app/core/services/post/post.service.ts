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
    const url = `${environment.apiUrl}/api/Post/GetPost?postID=${postID}`
    return this._httpClient.get<PostDto>(url);
  }
  createNewPost(body:any):Observable<any>{
    const url = `${environment.apiUrl}/api/Post/PublishPost`
    return this._httpClient.post<any>(url, body);
  }

  getPosts(currentPage:number, parameter?:string ):Observable<any>{
    const url = `${environment.apiUrl}/api/Post/GetPosts?${parameter}&page=${currentPage}`
    return this._httpClient.get<any>(url)
  }
  likePost(postGuid?:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Post/LikePost?postGuid=${postGuid}`
    return this._httpClient.post(url,{});
  }
  deletePost(postID:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Post/DeletePost?postID=${postID}`;
    return this._httpClient.delete<any>(url)
  }
  savePost(username:string, postID:string){
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
