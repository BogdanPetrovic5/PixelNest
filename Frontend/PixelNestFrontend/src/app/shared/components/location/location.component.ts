import { Component } from '@angular/core';

import axios from 'axios';

import { Map } from '@maptiler/sdk'; 
import { UserStateService } from 'src/app/core/services/states/user-state.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';


@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.scss']
})
export class LocationComponent {
  private map: any;
  private geocodeUrl = 'https://api.maptiler.com/geocoding/';
  private apiKey = 'aqR39NWYQyZAdFc6KtYh'
  suggestions: any[] = [];
  location = "";
  constructor(private _dashboardState:DashboardStateService){

  }
  ngOnInit(): void {
   
    this._dashboardState.location$.subscribe({
      next:response =>{
        this.location = response;
        console.log(response);
        this.initializeMap()
      }
    })
  }

  private async getCoordinates(location: string): Promise<[number, number] | null> {
    try {
      const requestUrl = `${this.geocodeUrl}${encodeURIComponent(location)}.json`;
      console.log(requestUrl)
      const response = await axios.get(requestUrl, {
        params: {
          key: this.apiKey, 
          limit: 1 
        }
      });
      console.log('API Response:', response.data);


      const features = response.data.features;
      if (Array.isArray(features) && features.length > 0) {
        const center = features[0].geometry.coordinates;
        console.log('Coordinates:', center); 
        return center; 
      } else {
        console.warn('No features found in response or features is not an array.');
        return null;
      }
    } catch (error:any) {
      console.error('Error fetching coordinates:', error.response ? error.response.data : error.message);
      return null;
    }
  }

  private async initializeMap(): Promise<void> {
    const coordinates = await this.getCoordinates(this.location);

    const center: [number, number] = coordinates ? coordinates : [0, 0];
    if (typeof (window as any).Map === 'undefined') {
      console.error("MapTiler map library is not loaded.");
      return;
    }
    this.map = new Map({
      container: 'map',
      style: `https://api.maptiler.com/maps/basic-v2/style.json?key=${this.apiKey}`, 
      center: center, 
      zoom: 12
    });
    
  }
 
}
