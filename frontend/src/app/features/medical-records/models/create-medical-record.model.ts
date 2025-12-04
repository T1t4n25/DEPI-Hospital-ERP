export interface CreateMedicalRecordModel {
  patientID: number;
  doctorID: number;
  diagnosesid: number;
  diagnoseDate: string; // ISO date string (YYYY-MM-DD)
}
