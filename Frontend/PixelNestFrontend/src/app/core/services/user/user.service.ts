import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { FollowersDto } from '../../dto/followers.dto';
import { FollowingsDto } from '../../dto/followings.dto';
import { Users } from '../../dto/users.dto';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _httpClient:HttpClient) { }
  updateLocation(location:any):Observable<any>{
    const url = `${environment.apiUrl}/api/user/location`
    return this._httpClient.patch(url, location);
  }
  getUserData(clientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/user/${clientGuid}`
    return this._httpClient.get(url);
  }

  getFollowers(clientGuid:string):Observable<FollowersDto[]>{
    const url = `${environment.apiUrl}/api/user/${clientGuid}/followers`
    return this._httpClient.get<FollowersDto[]>(url);
  }

  getFollowings(clientGuid:string):Observable<FollowingsDto[]>{
    const url = `${environment.apiUrl}/api/user/${clientGuid}/followings`
    return this._httpClient.get<FollowingsDto[]>(url);
  }

  isFollowing(targetClientGuid:string):Observable<boolean>{
    const url = `${environment.apiUrl}/api/user/followings/${targetClientGuid}`
    return this._httpClient.get<boolean>(url);
  }

  follow(targetClientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/user/follow/${targetClientGuid}`
    return this._httpClient.post<any>(url,{})
  }


  changeProfilePicture(formData:FormData):Observable<boolean>{
    const url = `${environment.apiUrl}/api/user/profile-picture`

    return this._httpClient.put<boolean>(url,formData);
  }
  changeUsername(formData:FormData):Observable<boolean>{
    const url = `${environment.apiUrl}/api/user/username`
    return this._httpClient.put<boolean>(url,formData)
  }
  
  getProfilePicture(clientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/user/profile-picture/${clientGuid}`

    return this._httpClient.get<any>(url);
  }
  findUsers(username:string):Observable<Users[]>{
    const url = `${environment.apiUrl}/api/user/search?username=${username}`

    return this._httpClient.get<Users[]>(url);
  }

  getCurrentUserData(){
    const url = `${environment.apiUrl}/api/user/me`
    return this._httpClient.get<any>(url);
  }

}
