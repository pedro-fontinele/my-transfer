/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RotinaService } from './rotina.service';

describe('Service: Rotina', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RotinaService]
    });
  });

  it('should ...', inject([RotinaService], (service: RotinaService) => {
    expect(service).toBeTruthy();
  }));
});
