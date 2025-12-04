export interface MedicalRecordListModel {
  recordID: number;
  patientName: string;
  doctorName: string;
  diagnosis: string;
  diagnoseDate: string; // ISO date string (YYYY-MM-DD)
}
