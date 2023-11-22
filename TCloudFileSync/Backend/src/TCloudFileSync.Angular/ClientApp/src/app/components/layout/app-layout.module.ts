import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppLayoutComponent } from './app-layout.component';
import { PoTemplatesModule } from '@po-ui/ng-templates';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { PoModule } from '@po-ui/ng-components';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppMenuComponent } from './application/menu/menu/menu.component';

@NgModule({
  declarations: [
    AppLayoutComponent,
    AppMenuComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    CommonModule,
    RouterModule,
    PoModule,
    PoTemplatesModule,
    BrowserAnimationsModule
  ],
  exports: [
    AppLayoutComponent
  ]
})
export class AppLayoutModule { }
