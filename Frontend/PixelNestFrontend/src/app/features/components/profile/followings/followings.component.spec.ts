import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FollowingsComponent } from './followings.component';

describe('FollowingsComponent', () => {
  let component: FollowingsComponent;
  let fixture: ComponentFixture<FollowingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FollowingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FollowingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
