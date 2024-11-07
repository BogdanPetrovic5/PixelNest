import { TestBed } from '@angular/core/testing';

import { StoryStateService } from './story-state.service';

describe('StoryStateService', () => {
  let service: StoryStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StoryStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
