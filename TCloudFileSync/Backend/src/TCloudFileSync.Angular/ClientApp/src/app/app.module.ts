import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppLayoutModule } from './components/layout/app-layout.module';
import { BaseUrlService, obterConfiguracoesApp } from './service/base-url/base-url.service';

@NgModule({
    declarations: [
      AppComponent
    ],
    imports: [
      AppRoutingModule,
      AppLayoutModule,
      BrowserAnimationsModule
    ],
    providers: [
      {
        provide: APP_INITIALIZER,
        useFactory: obterConfiguracoesApp,
        deps: [BaseUrlService],
        multi: true
      }
    ],
    bootstrap: [
      AppComponent
    ],
    schemas: [
      CUSTOM_ELEMENTS_SCHEMA
    ]
})
export class AppModule { }
