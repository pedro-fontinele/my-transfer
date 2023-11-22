import { RouterModule } from "@angular/router";
import { AppConfiguraSftpComponent } from "./configuracao-sftp.component";
import { NgModule } from "@angular/core";

@NgModule({
	imports: [RouterModule.forChild([
		{ path: '', component: AppConfiguraSftpComponent }
		
	])],
	exports: [RouterModule]
})
export class PageConfiguracaoSftpRouting { }
