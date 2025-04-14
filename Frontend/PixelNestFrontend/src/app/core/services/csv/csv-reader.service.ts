import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as Papa from 'papaparse';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class CsvReaderService {

  constructor(private _httpClient:HttpClient) { }

  getCities(selectedCountry:string):Observable<any>{

    return new Observable(observer =>{
      this._httpClient.get('assets/data/worldcities.csv', { responseType: 'text' })
      .subscribe(csvData => {
        Papa.parse(csvData, {
          header: true,
          skipEmptyLines: true,
          complete: (result) => {
         
            const cities = result.data
            .filter((city: any) => city.country === selectedCountry)
            .map((city: any) => city.city);
            observer.next(cities);
            observer.complete();
          }
        });
      });
    })
    
  }
}
