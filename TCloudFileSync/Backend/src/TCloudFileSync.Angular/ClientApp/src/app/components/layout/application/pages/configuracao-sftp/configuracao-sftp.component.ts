import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { PoModalAction , PoModalComponent, PoNotification, PoNotificationService, PoPageAction, PoSelectOption } from '@po-ui/ng-components';
import { PoBreadcrumbItens } from 'src/app/common/helpers/class/breadcrumb/breadcrumb-itens';
import { SentidoDoSincronismo } from 'src/app/common/helpers/class/sentido-do-sincronismo/sentido-do-sincronismo';
import { ColunasTabela } from 'src/app/common/helpers/class/colunas-tabela/colunas-tabela';
import { Rotina } from 'src/app/domain/entity/rotina/rotina';
import { ConfiguracaoSFTP } from 'src/app/domain/entity/configuracao-sftp/configuracao-sftp';
import { ConfiguracaosSFTPService } from 'src/app/service/configuracao-sftp/configuracao-sftp.service';
import { EntidadeBase } from 'src/app/domain/entity/entidade-base/entidade-base';
import { OpcaoSimOuNao } from 'src/app/common/helpers/class/opcao-sim-ou-nao/opcao-sim-ou-nao';
import { RotinaService } from 'src/app/service/rotina/rotina.service';
import { TypeNotifications } from 'src/app/common/helpers/class/type-notifications/type-notifications';
import { ValidatorRotina } from 'src/app/common/validator/validator-rotina/validator-rotina';


@Component({
    selector: 'app-configuracao',
    templateUrl: './configuracao-sftp.component.html'
})
export class AppConfiguraSftpComponent implements OnInit {
    @ViewChild('formsConfiguracaoSFTP', { static: true }) public formsConfiguracaoSFTP?: NgForm;
    @ViewChild('formAddOuEditarRotina', { static: true }) public formAddOuEditarRotina?: NgForm;
    @ViewChild('modalAddOuEditarRotina', { static: true }) public modalAddOuEditarRotina?: PoModalComponent;

    // PO Models
    public pActionsForm: Array<PoPageAction> = [];
    public pPoModalAction: PoModalAction;
    public pActionsTable: Array<PoPageAction> = [];
    public opcaoSentidoDoSincronismo: Array<PoSelectOption> = SentidoDoSincronismo.sentidoDoSincronismo;
    public opcaoSimNao: Array<PoSelectOption> = OpcaoSimOuNao.opcaoSimOuNao;

    public poBreadcrumbSettings = PoBreadcrumbItens.poBreadcrumbSettings;
    public columns = ColunasTabela.colunasTabelaConfiguracaoSftp;
    public configuracaoSFTP = new ConfiguracaoSFTP();
    public rotina = new Rotina();
    public entidadeBase = new EntidadeBase();
    public items: Array<any> = [];
    public id: number = 0;

    constructor(public configuracaoSFTPService: ConfiguracaosSFTPService,
                public rotinaService: RotinaService,
                public poNotification: PoNotificationService) {
        this.pActionsForm = [
          {
            action: this.salvarConfiguracao.bind(this),
            label: `Salvar`,
            icon: 'po-icon-ok'
          },
          {
            action: this.cancelarConfiguracoes.bind(this),
            label: `Cancelar`
          }
        ]

        this.pActionsTable = [
          {
            action: this.ativarRotina.bind(this),
            label: "Ativar",
            icon: 'po-icon po-icon-ok',
            disabled: this.desabilitaAtivarRotina.bind(this)
          },
          {
            action : this.inativarRotina.bind(this),
            label: 'Inativar',
            icon: 'po-icon po-icon-clear-content',
            disabled: this.desabilitaInativarRotina.bind(this)
          },
          {
            action: this.editarRotina.bind(this),
            label: 'Editar',
            icon: 'po-icon po-icon-edit'
          }
        ]

        this.pPoModalAction = {
            action: this.salvarRotina.bind(this),
            label: `Salvar`
        }   
    }

    public ngOnInit(): void {
      this.getConfiguracaoSFTP();
    }

    public getConfiguracaoSFTP(): void{
      this.configuracaoSFTPService.getConfiguracaoSFTP().subscribe(
          (response: EntidadeBase) => {
            if (response?.configuracaoClienteSftp && response?.configuracaoEnvioArquivo) {
              this.configuracaoSFTP = response.configuracaoClienteSftp;
              this.items = response.configuracaoEnvioArquivo;
            }
          },
          (error: any) => {
            console.log(error);
          }
      )
    }

    public getTodasRotinas(): void{
      this.rotinaService.getTodasRotinas().subscribe(
        (response: Rotina[]) => {
            this.items = response;
        },
        (error: any) => {
          const errors = error.error.errors;
          for (const key in errors) {
          if (errors.hasOwnProperty(key)) {
               let errorMessages = errors[key];
               this.showNotification(TypeNotifications.Error, errorMessages);
             }
          }
        }
      )
    }

    public editarRotina($event: any): void{
      this.getRotinaPorId($event.id);
      this.abrirModalEditarRotina($event.id);
    }

    public ativarRotina($event: any): void{
      $event.ativo = 'true';
      $event.idConfiguracaoSftp = 0;
      this.putRotina($event, false);
    }

    public inativarRotina($event: any): void{
      $event.ativo = 'false';
      $event.idConfiguracaoSftp = 0;
      this.putRotina($event, false);
    }

    public desabilitaAtivarRotina($event: any): boolean{
      return $event.ativo ? true : false;
    }

    public desabilitaInativarRotina($event: any): boolean{
      return $event.ativo ? false : true;
    }

    public abrirModalAddRotina(): void{
      this.id = 0;
      this.rotina = new Rotina();
      this.modalAddOuEditarRotina?.open();
    }

    public abrirModalEditarRotina(id: number): void{
      this.modalAddOuEditarRotina?.open();
      this.id = id;
    }
    
    public cancelarConfiguracoes(): void {
      this.getConfiguracaoSFTP(); 
    }

    public async salvarConfiguracao(): Promise<void> {
      if (this.formsConfiguracaoSFTP?.value != null) {
        const configuracaoSFTP = { ...this.formsConfiguracaoSFTP?.value };
        
        if (this.validaConfiguracaoSFTP(configuracaoSFTP)) {
          this.configuracaoSFTP = configuracaoSFTP;
          await this.putConfiguracao(this.configuracaoSFTP);
        }
      }
    }
    
    public async salvarRotina(): Promise<void> {
      if (this.formAddOuEditarRotina?.value != null) {
        const configuracaoSFTP = { ...this.formsConfiguracaoSFTP?.value };
        const rotina = { ...this.formAddOuEditarRotina?.value };
        
        if (this.validaRotina(rotina) && this.validaConfiguracaoSFTP(configuracaoSFTP)) {
          await this.salvarConfiguracao();
          this.rotina = rotina;
          this.rotina.id = this.id;          
          this.rotina.idConfiguracaoSftp = 0;
          await this.putRotina(this.rotina);
        }
      }
    }    

    public fecharModalRotina(): void{
      this.formAddOuEditarRotina?.reset();
      this.modalAddOuEditarRotina?.close();
    }  

    public async putRotina(rotina: Rotina, reloadAll: boolean = true): Promise<void> {  
      try {
        debugger;
        rotina = ValidatorRotina.validaRotinaFluxoServidor(rotina);      
        const response: any = await this.rotinaService.putRotina(rotina).toPromise();
        this.showNotification(TypeNotifications.Success, 'Rotina de mapeamento salva com sucesso.');
        this.zeraPonteiro()
        if(reloadAll) {
          await this.getConfiguracaoSFTP();
        }
        else {
          this.getTodasRotinas();
        }
        this.fecharModalRotina();  
      } catch (error) {
        this.handlePutError(error);
      }
    }
    
    public async putConfiguracao(configuracaoSFTP: ConfiguracaoSFTP): Promise<void> {
      try {
        const response: any = await this.configuracaoSFTPService.putConfiguracaoSFTP(configuracaoSFTP).toPromise();
        this.showNotification(TypeNotifications.Success, 'Configurações salvas com sucesso.');
        await this.getConfiguracaoSFTP();
      } catch (error) {
        this.handlePutError(error);
      }
    }
    
    private handlePutError(error: any): void {
      const errors = error.error.errors;
      for (const key in errors) {
        if (errors.hasOwnProperty(key)) {
          let errorMessages = errors[key];
          this.showNotification(TypeNotifications.Error, errorMessages);
        }
      }
    }

    public getRotinaPorId(id: number): void{
      this.rotinaService.getRotinaPorId(id).subscribe(
        (response: Rotina) => {
            this.rotina = ValidatorRotina.validaRotinaFluxoAplicacao(response);
        },
        (error: any) => {
          const errors = error.error.errors;
          for (const key in errors) {
          if (errors.hasOwnProperty(key)) {
               let errorMessages = errors[key];
               this.showNotification(TypeNotifications.Error, errorMessages);
             }
          }
        }
      )
    }

    private validaRotina(rotina: Rotina): boolean {
      if (!rotina.caminhoArquivoNuvem || rotina.caminhoArquivoNuvem.trim().length === 0) {
        this.showNotification(TypeNotifications.Warning, 'O caminho do arquivo nuvem é obrigatório.');
        return false;
      }
      
      if (!rotina.caminhoArquivoLocal || rotina.caminhoArquivoLocal.trim().length === 0) {
        this.showNotification(TypeNotifications.Warning, 'O caminho do arquivo local é obrigatório.');
        return false;
      }

      if (rotina.fluxoLocalParaNuvem == null) {
        this.showNotification(TypeNotifications.Warning, 'O sentido do fluxo é obrigatório');
        return false;
      }
      
      if (!rotina.tempoParaIniciarProximoEnvio) {
        this.showNotification(TypeNotifications.Warning, 'A recorrência em milissegundos é obrigatória');
        return false;
      }

      if (rotina.tempoParaIniciarProximoEnvio < 30000) {
        this.showNotification(TypeNotifications.Warning, 'A recorrência mínima é de 30000 milissegundos');
        return false;
      }
      
      if (rotina.deveApagarArquivoOrigem == null) {
        this.showNotification(TypeNotifications.Warning, 'Informe se deve-se apagar o arquivo da origem.');
        return false;
      }

      if (rotina.ativo == null) {
        this.showNotification(TypeNotifications.Warning, 'Informe o status deste mapeamento.');
        return false;
      }
      return true;
    }

    
    private zeraPonteiro(): void{
      this.rotina = new Rotina();
    }

    private validaConfiguracaoSFTP(configuracaoSFTP: ConfiguracaoSFTP): boolean {
      if (configuracaoSFTP.host == undefined || configuracaoSFTP.port == undefined || configuracaoSFTP.username == undefined || configuracaoSFTP.password == undefined ){
        this.showNotification(TypeNotifications.Warning, 'Preencha as configurações globais');
        return false;
      }

      if (!configuracaoSFTP.host || configuracaoSFTP.host.trim().length === 0) {
        this.showNotification(TypeNotifications.Warning, 'O campo Host é obrigatório.');
        return false;
      }
      
      if (!configuracaoSFTP.port) {
        this.showNotification(TypeNotifications.Warning, 'O campo Porta é obrigatório.');
        return false;
      }
      
      if (!configuracaoSFTP.username || configuracaoSFTP.username.trim().length === 0) {
        this.showNotification(TypeNotifications.Warning, 'O campo Username é obrigatório.');
        return false;
      }
      
      if (!configuracaoSFTP.password || configuracaoSFTP.password.trim().length === 0) {
        this.showNotification(TypeNotifications.Warning, 'O campo Password é obrigatório.');
        return false;
      }
      return true;
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