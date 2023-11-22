import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { AppHistoricoSincronismoComponent } from "./historico-sincronismo.component";

@NgModule({
	imports: [RouterModule.forChild([
		{ path: '', component: AppHistoricoSincronismoComponent }
		
	])],
	exports: [RouterModule]
})
export class PageHistoricoSincronismoRouting { }
