import { HttpClientModule } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { AgGridAngular } from 'ag-grid-angular';
import { AuditLogs } from './auditlogs/auditlogs';
import { Transactions } from './transactions/transactions';

@NgModule({
  declarations: [
    App, AuditLogs, Transactions
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, AgGridAngular
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
  ],
  bootstrap: [App, AuditLogs, Transactions]
})
export class AppModule { }
