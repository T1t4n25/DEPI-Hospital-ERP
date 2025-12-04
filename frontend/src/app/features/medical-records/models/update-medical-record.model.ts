import { CreateMedicalRecordModel } from './create-medical-record.model';

export interface UpdateMedicalRecordModel extends CreateMedicalRecordModel {
  recordID: number;
}
