import { TestBed } from '@angular/core/testing';

import { AuditoriaApi } from './auditoria-api';

describe('AuditoriaApi', () => {
  let service: AuditoriaApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuditoriaApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
