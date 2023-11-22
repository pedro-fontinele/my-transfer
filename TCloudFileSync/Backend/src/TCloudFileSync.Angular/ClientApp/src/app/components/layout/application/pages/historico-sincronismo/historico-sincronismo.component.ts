import { Component, OnInit } from '@angular/core';
import { PoNotification, PoNotificationService } from '@po-ui/ng-components';
import { PoBreadcrumbItens } from 'src/app/common/helpers/class/breadcrumb/breadcrumb-itens';
import { ColunasTabela } from 'src/app/common/helpers/class/colunas-tabela/colunas-tabela';
import { TypeNotifications } from 'src/app/common/helpers/class/type-notifications/type-notifications';
import { HistoricoSincronismo } from 'src/app/domain/entity/historico-sincronismo/historico-sincronismo';
import { HistoricoSincronismoService } from 'src/app/service/historico-sincronismo/historico-sincronismo.service';

@Component({
  selector: 'app-historico',
  templateUrl: './historico-sincronismo.component.html'
})
export class AppHistoricoSincronismoComponent implements OnInit {

  public poBreadcrumbHistoric = PoBreadcrumbItens.poBreadcrumbHistoric;
  public columns = ColunasTabela.colunasTabelaHistoricoSincronismo;
  public items: any[] = [];
  public countPagina: number = 1;

  constructor(public poNotification: PoNotificationService,
              public historicoSincronismoService: HistoricoSincronismoService) {}

  public ngOnInit(): void { 
    this.atualizaHistorico();
    this.countPagina = 1;
  }

  public atualizaHistorico(pagina: number = 1, itensPorPagina: number= 20, deveConcatenar: boolean = false): void{
    this.historicoSincronismoService.buscaHistoricoSincronismo(pagina, itensPorPagina).subscribe({
      next: (response: HistoricoSincronismo[]) => {
        this.items = deveConcatenar ? this.items.concat(response) : response;
        this.countPagina = pagina;
      },
      error: (error: any) => {
        console.log(error);
        this.showNotification(TypeNotifications.Error, 'Ocorreu um erro na consultado historico de integração.');
      },
      complete: () => {
      }
    })
  }

  public carregaMaisResultados(): void{
    this.countPagina += 1;
    this.atualizaHistorico(this.countPagina, 20, true);
  }

  public disabilitaBotao(value: any[]): boolean {
    return value.length == 0;
  }

  public showNotification(type: string, notificacao: string) {
    const poNotification: PoNotification = {
      message: notificacao,
      duration: 4000
    };

    switch (type) {
      case TypeNotifications.Success:
        this.poNotification.success(poNotification);
        break;
      case TypeNotifications.Error:
        this.poNotification.error(poNotification);
        break;
      case TypeNotifications.Warning:
        this.poNotification.warning(poNotification);
        break;
      case TypeNotifications.Information:
        this.poNotification.information(poNotification);
        break;
    }
  }
}
