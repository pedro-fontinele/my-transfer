import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayoutComponent } from './components/layout/app-layout.component';


const routes: Routes = [
  {
    path: '', component: AppLayoutComponent,
    children: [
      { path: 'configuracoes', loadChildren: () => import('./components/layout/application/pages/configuracao-sftp/configuracao-sftp.module').then((m) => m.AppConfiguracaoSftpModule) },
      { path: 'historico', loadChildren: () => import('./components/layout/application/pages/historico-sincronismo/historico-sincronismo.module').then((m) => m.AppHistoricoSincronismoModule) },
      { path: '**', redirectTo: 'configuracoes' },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'enabled',
      anchorScrolling: 'enabled',
      onSameUrlNavigation: 'reload',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
