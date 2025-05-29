import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AnalyticsComponent } from './analytics.component';
import { of } from 'rxjs';
import { ChartComponent } from 'src/app/shared/components/chart/chart.component';
import { NgChartsModule } from 'ng2-charts';
import { AnalyticsService } from 'src/app/core/services/analytics/analytics.service';


describe('AnalyticsComponent (unit test)', () => {
  let component: AnalyticsComponent;
  let fixture: ComponentFixture<AnalyticsComponent>;
  let mockAnalyticsService:any

  beforeEach(async () => {
    mockAnalyticsService = {
      getLocationAnalytics:jasmine.createSpy().and.returnValue(of([
        {
          country: 'Germany',
          count: 10,
          cities: ['Berlin', 'Munich']
        },
        {
          country: 'France',
          count: 5,
          cities: ['Paris']
        }
      ]))
    }
    await TestBed.configureTestingModule({
      declarations: [ AnalyticsComponent, ChartComponent ],
      imports: [HttpClientTestingModule, NgChartsModule],
      providers:[
        {provide:AnalyticsService, useValue:mockAnalyticsService}
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('it shoudl call getLocationAnalytics on init and setChartData', ()=>{
    //This checks if method getLocationAnalytics is called upon component initialization.
    expect(mockAnalyticsService.getLocationAnalytics).toHaveBeenCalled();
    //It is expected of component to set up chartData with data from service.
    expect(component.chartData).toEqual([{
          country: 'Germany',
          count: 10,
          cities: ['Berlin', 'Munich']
        },
        {
          country: 'France',
          count: 5,
          cities: ['Paris']
        }]);
  })

});
