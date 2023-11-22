/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ConfiguracaosSFTPService } from './configuracao-sftp.service';

describe('Service: Settings', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ConfiguracaosSFTPService]
    });
  });

  it('should ...', inject([ConfiguracaosSFTPService], (service: ConfiguracaosSFTPService) => {
    expect(service).toBeTruthy();
  }));
});
