import { EXmlHttpRequestReadyState } from "../../common/helpers/enum/xmlHttpRequestReadyState-enum/xmlHttpRequestReadyState-enum";
import { TRotaAmbiente, TRotaServidorAplicacao } from "../../common/helpers/enum/rota-ambiene-enum/rota-ambiente";
import { TFuncao } from "src/app/common/helpers/type/funcao-type/funcao-type";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class BaseUrlService {
    public configuracoes!: TRotaServidorAplicacao;

    public get obterConfiguracoes(): TRotaServidorAplicacao {
      return this.configuracoes;
    }

    public obterUrl(): any{
      const configuracoesAmbiente = this.configuracoes.rotaServidor.apiBaseUrl;
      return configuracoesAmbiente;
    }

    constructor() { }

    public setConfiguracoes(configuracoes: TRotaServidorAplicacao): void {
      this.configuracoes = configuracoes;
    }

    public carregarConfiguracoes(): Promise<TRotaServidorAplicacao> {
    return new Promise((resolve, reject) => {
      const _self = this;
      const xhttp = new XMLHttpRequest();
      xhttp.responseType = 'json';
      xhttp.onreadystatechange = function (): void {
        if (this.readyState === EXmlHttpRequestReadyState.Done && this.status === 200) {
          _self.setConfiguracoes(xhttp.response);
          resolve(xhttp.response);
        }
        else if (this.readyState === EXmlHttpRequestReadyState.Done && this.status !== 200) {
          reject(`Arquivo de configuração não encontrado`);
        }
      };
      xhttp.open('GET', 'assets/config.json', true);
      xhttp.send();
    });
  }
}

export function obterConfiguracoesApp(baseUrlService: BaseUrlService): TFuncao<any> {
  return () => baseUrlService.carregarConfiguracoes();
}


