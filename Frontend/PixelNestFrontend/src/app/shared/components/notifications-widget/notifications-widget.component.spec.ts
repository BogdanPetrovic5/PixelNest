import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationsWidgetComponent } from './notifications-widget.component';

describe('NotificationsWidgetComponent', () => {
  let component: NotificationsWidgetComponent;
  let fixture: ComponentFixture<NotificationsWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NotificationsWidgetComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NotificationsWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
