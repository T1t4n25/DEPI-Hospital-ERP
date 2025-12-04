export interface CreateInvoiceModel {
  patientID: number;
  invoiceTypeID: number;
  invoiceDate: string; // ISO date string (YYYY-MM-DD)
  paymentStatusID: number;
  hospitalItems: CreateHospitalInvoiceItemModel[];
  medicationItems: CreateMedicationInvoiceItemModel[];
}

export interface CreateHospitalInvoiceItemModel {
  serviceID: number;
  lineTotal: number;
}

export interface CreateMedicationInvoiceItemModel {
  medicationID: number;
  quantity: number;
  lineTotal: number;
}

