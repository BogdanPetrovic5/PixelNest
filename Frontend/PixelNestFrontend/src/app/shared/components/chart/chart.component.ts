import { Component, Input } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss']
})
export class ChartComponent {
  @Input() chartData:any
  barChartOptions: ChartOptions = {
    responsive: true,
    indexAxis:'y'
  };

  barChartLabels = ['January', 'February', 'March', 'April'];

  barChartData: ChartData<'bar'> = {
    labels: this.barChartLabels,
   
    datasets: [
      { data: [10, 20, 30, 40], label: 'Users' }
    ]
  };
}
