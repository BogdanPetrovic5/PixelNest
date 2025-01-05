import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogOutDialogComponent } from './log-out-dialog.component';

describe('LogOutDialogComponent', () => {
  let component: LogOutDialogComponent;
  let fixture: ComponentFixture<LogOutDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LogOutDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LogOutDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
