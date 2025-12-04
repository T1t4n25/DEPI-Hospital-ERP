export interface InvoiceDetailModel {
    invoiceID: number;
    patientID: number;
    patientName: string;
    invoiceTypeID: number;
    invoiceTypeName: string;
    invoiceDate: string;
    totalAmount: number;
    paymentStatusID: number;
    paymentStatusName: string;
    payDate?: string;
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

export interface InvoiceListModel {
    invoiceID: number;
    patientName: string;
    invoiceTypeName: string;
    invoiceDate: string;
    totalAmount: number;
    paymentStatusName: string;
}

export interface CreateInvoiceModel {
    patientID: number;
    invoiceTypeID: number;
    invoiceDate: string;
    totalAmount: number;
    paymentStatusID: number;
}

export interface UpdateInvoiceModel {
    invoiceID: number;
    paymentStatusID: number;
    payDate?: string;
}
