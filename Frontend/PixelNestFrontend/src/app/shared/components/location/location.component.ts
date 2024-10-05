import { Component } from '@angular/core';

import axios from 'axios';

import { Map, Marker } from '@maptiler/sdk'; 
import { UserStateService } from 'src/app/core/services/states/user-state.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import maplibregl from 'maplibre-gl';
import { ActivatedRoute, Router } from '@angular/router';
import { PostDto } from 'src/app/core/dto/post.dto';
import { PostService } from 'src/app/core/services/post/post.service';


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

  posts:PostDto[] = []
  constructor(
    private _dashboardState:DashboardStateService,
    private _router:Router,
    private _route:ActivatedRoute,
    private _postService:PostService
  ){

  }
  ngOnInit(): void {
   
    this._dashboardState.location$.subscribe({
      next:response =>{
        console.log(response)
        if(response == null){
          this._route.params.subscribe(params => {
            const data = params['location'];
            console.log(data);
            this.location = data
            this.loadPosts();
            this.initializeMap();
          });
        }else{
          this.location = response;
          this.initializeMap()
        }
        
      }
    })
  }
  private loadPosts(){
    this._postService.getPostsByLocation(this.location).subscribe({
      next:response=>{
        this.posts = response
        console.log(this.posts);
      },
      error:error=>{
        console.log(error.message)
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
    this.map = new maplibregl.Map({
      container: 'map',
      style: `https://api.maptiler.com/maps/basic-v2/style.json?key=${this.apiKey}`, 
      center: center, 
      zoom: 8
    });
 
    const marker = new maplibregl.Marker({
     
      
    }).setLngLat(center)
    .addTo(this.map);

  } 
  private setMarker(center:[number, number]){
    
  }
}
