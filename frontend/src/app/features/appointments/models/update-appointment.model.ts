import { CreateAppointmentModel } from './create-appointment.model';

export interface UpdateAppointmentModel extends CreateAppointmentModel {
  appointmentID: number;
}
