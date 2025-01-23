import { TestBed } from '@angular/core/testing';

import { UnauthorizedAccessInterceptor } from './unauthorized-access.interceptor';

describe('UnauthorizedAccessInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      UnauthorizedAccessInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: UnauthorizedAccessInterceptor = TestBed.inject(UnauthorizedAccessInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
