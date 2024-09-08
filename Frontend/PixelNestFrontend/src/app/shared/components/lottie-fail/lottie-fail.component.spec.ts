import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LottieFailComponent } from './lottie-fail.component';

describe('LottieFailComponent', () => {
  let component: LottieFailComponent;
  let fixture: ComponentFixture<LottieFailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LottieFailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LottieFailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
