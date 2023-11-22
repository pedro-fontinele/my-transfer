import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppHistoricoSincronismoComponent } from './historico-sincronismo.component';
import { PageHistoricoSincronismoRouting } from './historico-sincronismo-routing.module';
import { PoModule } from '@po-ui/ng-components';
import { PoFieldModule } from '@po-ui/ng-components';

@NgModule({
  imports: [
    CommonModule,
    PoModule,
    PageHistoricoSincronismoRouting,
    PoFieldModule
  ],
  declarations: [AppHistoricoSincronismoComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AppHistoricoSincronismoModule { }
