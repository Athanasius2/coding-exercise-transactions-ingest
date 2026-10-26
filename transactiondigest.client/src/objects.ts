export interface Transaction {
  sourceSystem: string;
  transactionId: number;
  cardNumber: string | undefined;
  locationCode: string;
  productName: string;
  amount: number;
  transactionTime: Date;
}

export interface TransactionAuditLogs{
  id: number;
  sourceSystem: string;
  transactionId: number;
  fieldName: string;
  currentValue: string | null;
  updatedValue: string | null;
  updateTime: Date;
}

export interface OperationResult<T>{
  result: T | undefined;
  success: boolean;
  errorMessage: string;
}
