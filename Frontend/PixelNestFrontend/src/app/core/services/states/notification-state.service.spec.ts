import { TestBed } from '@angular/core/testing';

import { NotificationStateService } from './notification-state.service';

describe('NotificationStateService', () => {
  let service: NotificationStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NotificationStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
