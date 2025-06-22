import { TestBed } from '@angular/core/testing';

import { InboxStateService } from './inbox-state.service';

describe('InboxStateService', () => {
  let service: InboxStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InboxStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
