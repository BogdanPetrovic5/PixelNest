import { Component, OnInit } from '@angular/core';
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
  ngOnInit():void{
    this._analyticsService.getLocationAnalytics().subscribe({
      next:response=>{
      
        this.chartData = response;
        
      }, error: error =>{
        console.log(error);
      }

    })
  }
 
}
