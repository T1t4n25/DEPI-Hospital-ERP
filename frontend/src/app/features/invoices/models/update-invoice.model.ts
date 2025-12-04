export interface UpdateInvoiceModel {
  invoiceID: number;
  paymentStatusID: number;
  payDate?: string; // ISO date string (YYYY-MM-DD)
}

