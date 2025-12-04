export interface CreateAppointmentModel {
    patientID: number;
    doctorID: number;
    serviceID: number;
    appointmentDateTime: string;
    status: string;
}
