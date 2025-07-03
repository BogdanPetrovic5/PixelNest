import { TestBed } from '@angular/core/testing';

import { ChatFacadeService } from './chat-facade.service';

describe('ChatFacadeService', () => {
  let service: ChatFacadeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatFacadeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
