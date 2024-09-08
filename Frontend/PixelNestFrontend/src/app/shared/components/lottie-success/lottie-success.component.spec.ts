import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LottieSuccessComponent } from './lottie-success.component';

describe('LottieSuccessComponent', () => {
  let component: LottieSuccessComponent;
  let fixture: ComponentFixture<LottieSuccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LottieSuccessComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LottieSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
