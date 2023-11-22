import { Component, OnInit } from '@angular/core';
import { PoMenuItem } from '@po-ui/ng-components';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html'
})
export class AppMenuComponent implements OnInit {

  constructor() {}

  public ngOnInit(): void {}

  public plogo: string = 'assets/images/logo-branco.png';

  public menus: Array<PoMenuItem> = [
    { label: 'Agente Arquivos TCloud', icon: 'po-icon po-icon-device-desktop', link: '/configuracoes' },
    { label: 'Histórico De Sincronismo', icon: 'po-icon po-icon-change', link: '/historico' }
  ];

}
