import { CreateMedicationModel } from './create-medication.model';

export interface UpdateMedicationModel extends CreateMedicationModel {
  medicationID: number;
}

