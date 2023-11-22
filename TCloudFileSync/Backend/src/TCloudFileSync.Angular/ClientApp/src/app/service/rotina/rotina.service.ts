import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Rotina } from 'src/app/domain/entity/rotina/rotina';
import { BaseUrlService } from '../base-url/base-url.service';

@Injectable({
  providedIn: 'root'
})
export class RotinaService {

  constructor(private http: HttpClient, public baseUrl: BaseUrlService) { }
  
  public getTodasRotinas(): Observable<Rotina[]> {
    return this.http.get<Rotina[]>(`${this.baseUrl.obterUrl()}/api/rotina`).pipe(take(1));
  }

  public getRotinaPorId(id: number): Observable<Rotina> {
    return this.http.get<Rotina>(`${this.baseUrl.obterUrl()}/api/rotina/id/${id}`).pipe(take(1));
  }

  public putRotina(rotina: Rotina): Observable<Rotina> {
    return this.http.put<Rotina>(`${this.baseUrl.obterUrl()}/api/rotina`, rotina).pipe(take(1));
  }
}
