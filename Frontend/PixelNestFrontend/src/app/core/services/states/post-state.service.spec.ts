import { TestBed } from '@angular/core/testing';

import { PostStateService } from './post-state.service';

describe('PostStateService', () => {
  let service: PostStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PostStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
