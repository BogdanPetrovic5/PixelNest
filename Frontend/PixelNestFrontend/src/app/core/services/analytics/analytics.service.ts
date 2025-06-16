import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {

  constructor(private _httpClient:HttpClient) { }

  getLocationAnalytics():Observable<any>{
    const url = `${environment.apiUrl}/api/analytics/locations`
     return this._httpClient.get(url);
  }
}
