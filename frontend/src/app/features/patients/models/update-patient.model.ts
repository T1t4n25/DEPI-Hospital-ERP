import { CreatePatientModel } from './create-patient.model';

export interface UpdatePatientModel extends CreatePatientModel {
  patientID: number;
}

