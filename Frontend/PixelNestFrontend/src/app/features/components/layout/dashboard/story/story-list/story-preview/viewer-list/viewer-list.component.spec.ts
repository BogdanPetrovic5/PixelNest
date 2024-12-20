import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewerListComponent } from './viewer-list.component';

describe('ViewerListComponent', () => {
  let component: ViewerListComponent;
  let fixture: ComponentFixture<ViewerListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewerListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
