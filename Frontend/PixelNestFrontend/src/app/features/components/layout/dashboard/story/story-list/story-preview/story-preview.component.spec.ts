import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoryPreviewComponent } from './story-preview.component';

describe('StoryPreviewComponent', () => {
  let component: StoryPreviewComponent;
  let fixture: ComponentFixture<StoryPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StoryPreviewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StoryPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
