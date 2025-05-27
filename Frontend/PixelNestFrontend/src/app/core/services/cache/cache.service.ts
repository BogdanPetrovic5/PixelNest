import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CacheService {

  private _isChanged = new BehaviorSubject<boolean>(true)
  
  constructor(
    private _httpClient:HttpClient
  ) { }
  setCacheState(value:boolean){
    this._isChanged.next(value)
  }
  getValue() :boolean{
    return this._isChanged.getValue();
  }
  checkCache():Observable<boolean | null>{
    const url = `${environment.apiUrl}/api/post/cache/state`;
    return this._httpClient.get<boolean |null>(url);
  }
}
