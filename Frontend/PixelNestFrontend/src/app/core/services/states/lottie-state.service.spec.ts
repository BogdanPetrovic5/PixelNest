import { TestBed } from '@angular/core/testing';

import { LottieStateService } from './lottie-state.service';

describe('LottieStateService', () => {
  let service: LottieStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LottieStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
