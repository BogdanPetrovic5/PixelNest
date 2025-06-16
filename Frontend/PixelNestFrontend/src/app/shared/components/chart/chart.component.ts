import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ChartData, ChartOptions,  } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss']
})
export class ChartComponent implements OnChanges{
 
  @Input() chartData: { key: string; count: number }[] = [];
  @Input() chartDataLabel: string = '';
  barChartOptions: ChartOptions = {
    responsive: true,
    indexAxis: 'y',
    plugins: {
     
      tooltip: {
        callbacks: {
          label: context => `${context.label}: ${context.parsed.x}%`
        }
      },
     
    },
    scales: {
      x: {
        max: 100,
        ticks: {
          callback: val => val + '%'
        },
        grid: {
          color: '#444'
        }
      },
      y: {
        ticks: {
          color: 'white'
        },
       
      }
    }
  };


  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: []
  };
 
  ngOnChanges(changes: SimpleChanges): void {
    if(changes['chartData'] && this.chartData.length > 0){
      const total = this.chartData.reduce((sum, entry) => sum + entry.count, 0);

      const labels = this.chartData.map(chart => chart.key)
      const data = this.chartData.map(chart => +((chart.count / total) * 100).toFixed(1))
      this.barChartData = {
          labels: labels,
        
          datasets: [
            {
              data,
              label: `Top ${this.chartDataLabel}`
            }
          ]
      }
    }
  }
}
