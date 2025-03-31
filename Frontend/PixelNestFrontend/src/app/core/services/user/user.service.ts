import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { FollowersDto } from '../../dto/followers.dto';
import { FollowingsDto } from '../../dto/followings.dto';
import { Users } from '../../dto/users.dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private _httpClient:HttpClient) { }
  getUserData(clientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/User/GetUserProfile?clientGuid=${clientGuid}`
    return this._httpClient.get(url);
  }

  getFollowers(clientGuid:string):Observable<FollowersDto[]>{
    const url = `${environment.apiUrl}/api/User/GetFollowers?clientGuid=${clientGuid}`
    return this._httpClient.get<FollowersDto[]>(url);
  }

  getFollowings(clientGuid:string):Observable<FollowingsDto[]>{
    const url = `${environment.apiUrl}/api/User/GetFollowings?clientGuid=${clientGuid}`
    return this._httpClient.get<FollowingsDto[]>(url);
  }

  isFollowing(targetClientGuid:string):Observable<boolean>{
    const url = `${environment.apiUrl}/api/User/IsFollowing?targetClientGuid=${targetClientGuid}`
    return this._httpClient.get<boolean>(url);
  }

  follow(targetClientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/User/Follow?targetClientGuid=${targetClientGuid}`
    return this._httpClient.post<any>(url,{})
  }

  validateUser(postOwner:string):Observable<boolean>{
    const url = `${environment.apiUrl}/api/User/IsValid?postOwner=${postOwner}`

    return this._httpClient.get<boolean>(url);
  }

  changeProfilePicture(formData:FormData):Observable<boolean>{
    const url = `${environment.apiUrl}/api/User/ChangeProfilePicture`

    return this._httpClient.put<boolean>(url,formData);
  }
  changeUsername(formData:FormData):Observable<boolean>{
    const url = `${environment.apiUrl}/api/User/ChangeUsername`
    return this._httpClient.put<boolean>(url,formData)
  }
  
  getProfilePicture(clientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/User/GetProfilePicture?clientGuid=${clientGuid}`

    return this._httpClient.get<any>(url);
  }
  findUsers(username:string):Observable<Users[]>{
    const url = `${environment.apiUrl}/api/User/FindUsers?username=${username}`

    return this._httpClient.get<Users[]>(url);
  }

  getCurrentUserData(){
    const url = `${environment.apiUrl}/api/User/GetCurrentUserData`
    return this._httpClient.get<any>(url);
  }

}
