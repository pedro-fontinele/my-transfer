import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppConfiguraSftpComponent } from './configuracao-sftp.component';
import { PoModule } from '@po-ui/ng-components';
import { PageConfiguracaoSftpRouting } from './configuracao-sftp-routing.module';
import { FormsModule } from '@angular/forms';
import { PoFieldModule } from '@po-ui/ng-components';

@NgModule({
  imports: [
    CommonModule,
    PoModule,
    FormsModule,
    PageConfiguracaoSftpRouting,
    PoFieldModule
  ],
  declarations: [AppConfiguraSftpComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AppConfiguracaoSftpModule { }
