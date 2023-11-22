/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { HistoricoSincronismoService } from './historico-sincronismo.service';

describe('Service: HistoricoSincronismo', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HistoricoSincronismoService]
    });
  });

  it('should ...', inject([HistoricoSincronismoService], (service: HistoricoSincronismoService) => {
    expect(service).toBeTruthy();
  }));
});
