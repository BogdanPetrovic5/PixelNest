import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PixelnestLoadingComponent } from './pixelnest-loading.component';

describe('PixelnestLoadingComponent', () => {
  let component: PixelnestLoadingComponent;
  let fixture: ComponentFixture<PixelnestLoadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PixelnestLoadingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PixelnestLoadingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
