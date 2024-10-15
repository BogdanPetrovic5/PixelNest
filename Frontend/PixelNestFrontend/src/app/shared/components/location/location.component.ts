import { Component, OnDestroy, OnInit } from '@angular/core';
import { Map, Marker, geocoding, config } from '@maptiler/sdk'; 
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import maplibregl from 'maplibre-gl';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { PostDto } from 'src/app/core/dto/post.dto';
import { PostService } from 'src/app/core/services/post/post.service';
import { MapService } from 'src/app/core/services/map/map.service';
import { filter, Subscription, windowWhen } from 'rxjs';


@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.scss']
})
export class LocationComponent implements OnInit, OnDestroy{
  private map: any;
  private geocodeUrl = 'https://api.maptiler.com/geocoding/';
  private apiKey = 'aqR39NWYQyZAdFc6KtYh'
  
  suggestions: any[] = [];
  location = "";

  posts:PostDto[] = []
  subscribe:Subscription = new Subscription;
  constructor(
    private _dashboardState:DashboardStateService,
    private _router:Router,
    private _route:ActivatedRoute,
    private _postService:PostService,
    private _mapService:MapService
  ){
    config.apiKey = 'aqR39NWYQyZAdFc6KtYh'
  }
  ngOnInit(): void {
      this._initilizeApp();
  }
  ngOnDestroy(): void {
      this.subscribe.unsubscribe();
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
      const response = await geocoding.forward(location);
      const features = response.features;
      if (Array.isArray(features) && features.length > 0) {
        const geometry = features[0].geometry;
        if (geometry.type === "Point") {
            const coordinates = geometry.coordinates;
           
            const [longitude, latitude] = coordinates;
            return [longitude, latitude];
        } else {
          console.warn('No features found in response or features is not an array.');
          return null;
        } 
     } return null
  } catch (error:any) {
    console.error('Error fetching coordinates:', error.response ? error.response.data : error.message);
    return null;
  }
}
  private _initilizeApp(){
    this.subscribe.add(
      this._router.events.pipe(
        filter(event => event instanceof NavigationEnd)
      ).subscribe((response:any)=>{
        window.location.reload()
      })

      
    )
    this.subscribe.add(
      this._dashboardState.location$.subscribe({
        next:response =>{
        
          if(response == null){
            this._route.params.subscribe(params => {
              const data = params['location'];
              
              this.location = data
              this.loadPosts();
              this.initializeMap();
            });
          }else{
            this.location = response;
            this.loadPosts();
            this.initializeMap()
            
          }
          
        }
      })
    )
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
 
 
    this.setMarker(center);
    this._addNewMarker();
  } 

  private _addNewMarker(){
    this.map.on("click",async (event:maplibregl.MapMouseEvent & maplibregl.MapDataEvent) =>{
      const coordinates = event.lngLat;

      const marker = new maplibregl.Marker().setLngLat(coordinates).addTo(this.map)
      const results = await geocoding.reverse([coordinates.lng, coordinates.lat]);
      this._getLocationName(results)
      
    })
  }

  private _getLocationName(result:any) {
    if(result && Array.isArray(result.features) && result.features.length > 0){
      this.location = result.features[3]?.text_en; 
      this._router.navigate([`/Location/${this.location}`])
    }
  }
  private setMarker(center:[number, number]){
    const marker = new maplibregl.Marker({
     
      
    }).setLngLat(center)
    .addTo(this.map);
  }
}
