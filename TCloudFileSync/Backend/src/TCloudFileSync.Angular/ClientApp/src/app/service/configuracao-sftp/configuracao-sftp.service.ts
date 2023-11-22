import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { ConfiguracaoSFTP } from 'src/app/domain/entity/configuracao-sftp/configuracao-sftp';
import { EntidadeBase } from 'src/app/domain/entity/entidade-base/entidade-base';
import { BaseUrlService } from '../base-url/base-url.service';

@Injectable({
  providedIn: 'root'
})
export class ConfiguracaosSFTPService {
  
  constructor(private http: HttpClient, public baseUrl: BaseUrlService) {
  }

  public getConfiguracaoSFTP(): Observable<EntidadeBase> {
    return this.http.get<EntidadeBase>(`${this.baseUrl.obterUrl()}/api/configuracaoClient`).pipe(take(1));
  }

  public putConfiguracaoSFTP(configuracaoSFTP: ConfiguracaoSFTP): Observable<ConfiguracaoSFTP> {
    return this.http.put<ConfiguracaoSFTP>(`${this.baseUrl.obterUrl()}/api/configuracaoClient`, configuracaoSFTP).pipe(take(1));
  }
}
