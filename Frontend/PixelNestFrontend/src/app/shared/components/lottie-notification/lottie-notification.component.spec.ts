import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LottieNotificationComponent } from './lottie-notification.component';

describe('LottieNotificationComponent', () => {
  let component: LottieNotificationComponent;
  let fixture: ComponentFixture<LottieNotificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LottieNotificationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LottieNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
