import { Component, OnInit } from '@angular/core';
import { all } from 'axios';
import { ChartData, ChartOptions } from 'chart.js';
import { AnalyticsService } from 'src/app/core/services/analytics/analytics.service';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.scss']
})
export class AnalyticsComponent implements OnInit{
  constructor(private _analyticsService:AnalyticsService){}
  chartData:any
  countriesChart:any;
  citiesChart:any
  ngOnInit():void{
    this._analyticsService.getLocationAnalytics().subscribe({
      next:response=>{
      
        this.chartData = response;
        this._modifyChartToCountries();
        this._modifyChartToCities();
  
      }, error: error =>{
        console.log(error);
      }

    })
  }
  private _modifyChartToCountries(){
    this.countriesChart = this.chartData.map(({ country, count }: { country: string; count: number }) => ({
          key:country,
          count
    }));
  }
  private _modifyChartToCities(){
    const allCities = this.chartData.flatMap((entry:any) => entry.cities)

    const cityMap: {[city:string] :number} = {}

    allCities.forEach((city:any) => {
      cityMap[city] = (cityMap[city] || 0) + 1;
    })

    this.citiesChart = Object.entries(cityMap).map(([city, count]) => ({
      key:city,
      count
    }))
  }
}
