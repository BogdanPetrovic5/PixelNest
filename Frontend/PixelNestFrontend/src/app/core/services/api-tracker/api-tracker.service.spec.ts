import { TestBed } from '@angular/core/testing';

import { ApiTrackerService } from './api-tracker.service';

describe('ApiTrackerService', () => {
  let service: ApiTrackerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiTrackerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
