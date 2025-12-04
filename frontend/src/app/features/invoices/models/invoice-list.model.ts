export interface InvoiceListModel {
  invoiceID: number;
  patientName: string;
  invoiceTypeName: string;
  invoiceDate: string; // ISO date string (YYYY-MM-DD)
  totalAmount: number;
  paymentStatusName: string;
}

