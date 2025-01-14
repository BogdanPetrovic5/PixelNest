import { TestBed } from '@angular/core/testing';

import { ChatStateService } from './chat-state.service';

describe('ChatStateService', () => {
  let service: ChatStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
