export interface AppointmentListModel {
  appointmentID: number;
  patientName: string;
  doctorName: string;
  serviceName: string;
  appointmentDateTime: string; // ISO datetime string
  status: string;
}
