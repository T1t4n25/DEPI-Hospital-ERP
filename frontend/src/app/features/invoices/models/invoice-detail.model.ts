export interface InvoiceDetailModel {
  invoiceID: number;
  patientID: number;
  patientName: string;
  invoiceTypeID: number;
  invoiceTypeName: string;
  invoiceDate: string; // ISO date string (YYYY-MM-DD)
  totalAmount: number;
  paymentStatusID: number;
  paymentStatusName: string;
  payDate?: string; // ISO date string (YYYY-MM-DD)
  hospitalItems: HospitalInvoiceItemModel[];
  medicationItems: MedicationInvoiceItemModel[];
}

export interface HospitalInvoiceItemModel {
  invoiceItemID: number;
  serviceID: number;
  serviceName: string;
  lineTotal: number;
}

export interface MedicationInvoiceItemModel {
  invoiceItemID: number;
  medicationID: number;
  medicationName: string;
  quantity: number;
  lineTotal: number;
}

