import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveLocationComponent } from './save-location.component';

describe('SaveLocationComponent', () => {
  let component: SaveLocationComponent;
  let fixture: ComponentFixture<SaveLocationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SaveLocationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SaveLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
