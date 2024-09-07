import { TestBed } from '@angular/core/testing';

import { GlobalInterceptor } from './global.interceptor';

describe('GlobalInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      GlobalInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: GlobalInterceptor = TestBed.inject(GlobalInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
