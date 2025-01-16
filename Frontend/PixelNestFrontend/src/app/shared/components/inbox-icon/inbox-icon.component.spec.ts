import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InboxIconComponent } from './inbox-icon.component';

describe('InboxIconComponent', () => {
  let component: InboxIconComponent;
  let fixture: ComponentFixture<InboxIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InboxIconComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InboxIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
