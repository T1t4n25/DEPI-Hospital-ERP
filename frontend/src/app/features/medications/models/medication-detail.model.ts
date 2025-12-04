export interface MedicationDetailModel {
  medicationID: number;
  barCode: string;
  name: string;
  description: string;
  cost: number;
  quantity?: number;
  expiryDate?: string; // ISO date string (YYYY-MM-DD)
}

