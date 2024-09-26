import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LottieLoadingComponent } from './lottie-loading.component';

describe('LottieLoadingComponent', () => {
  let component: LottieLoadingComponent;
  let fixture: ComponentFixture<LottieLoadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LottieLoadingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LottieLoadingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
