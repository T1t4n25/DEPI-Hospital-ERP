export interface PatientDetailModel {
  patientID: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string; // ISO date string (YYYY-MM-DD)
  genderID: number;
  genderName: string;
  address: string;
  bloodTypeID: number;
  bloodTypeName: string;
  contactNumber: string;
}

