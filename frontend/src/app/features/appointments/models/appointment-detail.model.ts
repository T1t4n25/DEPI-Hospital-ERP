export interface AppointmentDetailModel {
    appointmentID: number;
    patientID: number;
    patientName: string;
    doctorID: number;
    doctorName: string;
    serviceID: number;
    serviceName: string;
    appointmentDateTime: string;
    status: string;
}
