import { TestBed } from '@angular/core/testing';

import { CsvReaderService } from './csv-reader.service';

describe('CsvReaderService', () => {
  let service: CsvReaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CsvReaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
