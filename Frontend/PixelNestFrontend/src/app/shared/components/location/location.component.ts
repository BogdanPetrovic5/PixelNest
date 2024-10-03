import { Component } from '@angular/core';

import axios from 'axios';

import { Map } from '@maptiler/sdk'; 


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
  location = 'Zmaj Jovina 5, Kragujevac';
  ngOnInit(): void {
    this.initializeMap();
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
  async onLocationInput(event: any) {
    const query = event.target.value;
    if (query.length > 2) { // Start searching after 2 characters
      const response = await axios.get(`${this.geocodeUrl}${encodeURIComponent(query)}.json`, {
        params: {
          key: this.apiKey,
          limit: 5 // Limit to 5 suggestions
        }
      });

      this.suggestions = response.data.features;
    } else {
      this.suggestions = [];
    }
  }

  selectLocation(suggestion: any) {
    // Set the map center to the selected location
    const center = suggestion.center;
    this.map.setCenter(center);
    this.map.setZoom(15); // Adjust zoom level for street view
    this.suggestions = []; // Clear suggestions after selection
  }
}
