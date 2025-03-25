import { TestBed } from '@angular/core/testing';

import { GoogleAuthenticationService } from './google-authentication.service';

describe('GoogleAuthenticationService', () => {
  let service: GoogleAuthenticationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GoogleAuthenticationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
