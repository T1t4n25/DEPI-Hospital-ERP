export interface CreatePatientModel {
  firstName: string;
  lastName: string;
  dateOfBirth: string; // ISO date string (YYYY-MM-DD)
  genderID: number;
  address: string;
  bloodTypeID: number;
  contactNumber: string;
}

