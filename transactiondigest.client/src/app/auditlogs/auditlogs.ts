import { HttpClient } from "@angular/common/http";
import { Component, OnInit, signal } from "@angular/core";
import { ColDef, GridReadyEvent } from "ag-grid-community";
import { OperationResult, TransactionAuditLogs } from '../../objects';

@Component({
  selector: "audit-logs",
  templateUrl: "./auditlogs.html",
  standalone: false,
})
export class AuditLogs implements OnInit{

  logs = signal(new Array<TransactionAuditLogs>());
  errorMessage = signal("");


  defaultColDef = {
    sortable: true,
    filter: true,
    resizable: true,
  };

  colDefs: ColDef[] = [
    {field: "sourceSystem"},
    {field: "transactionId"},
    {field: "fieldName"},
    {field: "currentValue"},
    {field: "updatedValue"},
    {field: "updateTime"},
  ]
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getLogs();
  }

  getLogs(){
    this.http.get<OperationResult<TransactionAuditLogs[]>>('/api/transactions/auditlogs').subscribe(
      (result) =>{
        if(!result.success){
          this.errorMessage.set(result.errorMessage);
        }
        else{
          this.logs.set(result.result!);
          this.errorMessage.set("");
        }
      }
    )
  }

  onGridReady(params: GridReadyEvent): void {
    params.api.sizeColumnsToFit();
  }
}
