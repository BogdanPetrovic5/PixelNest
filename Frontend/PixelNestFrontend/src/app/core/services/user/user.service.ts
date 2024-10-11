import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { FollowersDto } from '../../dto/followers.dto';
import { FollowingsDto } from '../../dto/followings.dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _httpClient:HttpClient) { }
  getUserData(username:string):Observable<any>{
    const url = `${environment.apiUrl}/api/User/GetUserProfile?username=${username}`
    return this._httpClient.get(url);
  }

  getFollowers(username:string):Observable<FollowersDto[]>{
    const url = `${environment.apiUrl}/api/User/GetFollowers?username=${username}`
    return this._httpClient.get<FollowersDto[]>(url);
  }

  getFollowings(username:string):Observable<FollowingsDto[]>{
    const url = `${environment.apiUrl}/api/User/GetFollowings?username=${username}`
    return this._httpClient.get<FollowingsDto[]>(url);
  }

  isFollowing(follower:string, following:string):Observable<boolean>{
    const url = `${environment.apiUrl}/api/User/IsFollowing?FollowerUsername=${follower}&FollowingUsername=${following}`
    return this._httpClient.get<boolean>(url);
  }

  follow(follower:string, following:string):Observable<any>{
    const url = `${environment.apiUrl}/api/User/Follow`
    return this._httpClient.post<any>(url,{
      FollowerUsername:follower,
      FollowingUsername:following
    })
  }
}
