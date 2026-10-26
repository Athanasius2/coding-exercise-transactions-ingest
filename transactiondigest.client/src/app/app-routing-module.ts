import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuditLogs } from './auditlogs/auditlogs';
import { Transactions } from './transactions/transactions';

const routes: Routes = [
  {path: "auditlogs", component: AuditLogs},
  {path: "", component: Transactions }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
