import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MapService {
  mapTilerApiKey:string = 'aqR39NWYQyZAdFc6KtYh';
  constructor(private _httpClient:HttpClient) { }

  getLocation(lng:number, lat:number):Observable<any>{
    const reverseGeocodingUrl = `https://api.maptiler.com/geocoding/${lng},${lat}.json?key=${this.mapTilerApiKey}`;


    return this._httpClient.get<any>(reverseGeocodingUrl, { withCredentials: false })
    
  }
}
