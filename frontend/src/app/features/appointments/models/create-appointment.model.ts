export interface CreateAppointmentModel {
  patientID: number;
  doctorID: number;
  serviceID: number;
  appointmentDateTime: string; // ISO datetime string
  status?: string; // Default: "Scheduled"
}
