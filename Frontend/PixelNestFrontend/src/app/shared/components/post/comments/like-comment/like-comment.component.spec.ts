import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LikeCommentComponent } from './like-comment.component';

describe('LikeCommentComponent', () => {
  let component: LikeCommentComponent;
  let fixture: ComponentFixture<LikeCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LikeCommentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LikeCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
