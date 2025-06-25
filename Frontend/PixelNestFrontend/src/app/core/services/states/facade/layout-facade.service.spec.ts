import { TestBed } from '@angular/core/testing';

import { LayoutFacadeService } from './layout-facade.service';

describe('LayoutFacadeService', () => {
  let service: LayoutFacadeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LayoutFacadeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
