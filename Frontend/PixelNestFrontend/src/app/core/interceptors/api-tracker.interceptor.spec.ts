import { TestBed } from '@angular/core/testing';

import { ApiTrackerInterceptor } from './api-tracker.interceptor';

describe('ApiTrackerInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      ApiTrackerInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: ApiTrackerInterceptor = TestBed.inject(ApiTrackerInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
