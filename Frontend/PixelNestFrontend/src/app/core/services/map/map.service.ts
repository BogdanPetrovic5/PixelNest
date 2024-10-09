import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MapService {
  mapTilerApiKey:string = 'aqR39NWYQyZAdFc6KtYh';
  apiKey: string = 'aqR39NWYQyZAdFc6KtYh'; 
  geocodeUrl: string = 'https://api.maptiler.com/geocoding/';
  constructor(private _httpClient:HttpClient) { }

  getLocation(lng:number, lat:number):Observable<any>{
    const reverseGeocodingUrl = `https://api.maptiler.com/geocoding/${lng},${lat}.json?key=${this.mapTilerApiKey}`;


    return this._httpClient.get<any>(reverseGeocodingUrl, { withCredentials: false })
    
  }
  getAllLocations(query:string):Observable<any>{
    const url = `${this.geocodeUrl}${encodeURIComponent(query)}.json`
    return this._httpClient.get<any>(url, {
      params:{
        key:this.apiKey,
        limit:5
      }
    })
  }
}
