export interface MedicalRecordDetailModel {
  recordID: number;
  patientID: number;
  patientName: string;
  doctorID: number;
  doctorName: string;
  diagnosesid: number;
  diagnosis: string;
  diagnoseDate: string; // ISO date string (YYYY-MM-DD)
}
