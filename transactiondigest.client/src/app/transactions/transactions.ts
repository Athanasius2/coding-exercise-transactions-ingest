import { HttpClient } from "@angular/common/http";
import { Component, OnInit, signal } from "@angular/core";
import {ColDef, GridReadyEvent} from "ag-grid-community";
import { OperationResult, Transaction} from '../../objects';


@Component({
  selector: 'transactions',
  standalone: false,
  templateUrl: './transactions.html',
})
export class Transactions implements OnInit {
  public transactions = signal(new Array<Transaction>());
  errorMessage = signal("");
  successMessage = signal("");

  defaultColDef = {
    sortable: true,
    filter: true,
    resizable: true,
  };

  colDefs: ColDef[] = [
    {field: "sourceSystem"},
    {field: "transactionId"},
    {field: "cardNumber"},
    {field: "locationCode"},
    {field: "productName"},
    {field: "amount"},
    {field: "transactionTime"},
  ]

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getTransactions();
  }


  onGridReady(params: GridReadyEvent): void {
    params.api.sizeColumnsToFit();
  }

  getTransactions() {
    this.http.get<OperationResult<Transaction[]>>('/api/transactions').subscribe(
      (result) => {
        if (!result.success){
          this.errorMessage.set(result.errorMessage);
          this.successMessage.set("");
        }
        else{
          this.transactions.set(result.result!);
          this.errorMessage.set("");
        }
      },
      (error) => {
        console.error(error);
      }
    );
  }

  import(){
    this.http.post<OperationResult<number>>('/api/import', null).subscribe(
      (result) => {
        if(!result.success){
          this.errorMessage.set(result.errorMessage);
          this.successMessage.set("");
        }
        else {
          this.successMessage.set(`${result.result} new transactions were created`);
          this.getTransactions();
        }
      }
    );
    this.getTransactions();
  }

  delete(){
    this.http.delete<OperationResult<null>>('/api/transactions').subscribe(
      (result) => {
        if(!result.success){
          this.errorMessage.set(result.errorMessage);
          this.successMessage.set("");
        }
        else {
          this.getTransactions();
        }
      }
    );
  }

}
