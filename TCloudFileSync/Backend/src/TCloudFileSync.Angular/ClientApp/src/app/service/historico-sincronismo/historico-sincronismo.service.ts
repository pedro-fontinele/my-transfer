import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrlService } from '../base-url/base-url.service';
import { HistoricoSincronismo } from 'src/app/domain/entity/historico-sincronismo/historico-sincronismo';
import { Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HistoricoSincronismoService {

    constructor(public http: HttpClient, 
                public baseUrl: BaseUrlService){
    }

    public buscaHistoricoSincronismo(pagina: number = 1, itensPorPagina: number= 20): Observable<HistoricoSincronismo[]>{
      return this.http.get<HistoricoSincronismo[]>(`${this.baseUrl.obterUrl()}/api/historico/pagina/${pagina}/itensPorPagina/${itensPorPagina}`).pipe(take(1));
    }
}
