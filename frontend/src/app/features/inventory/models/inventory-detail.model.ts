export interface InventoryDetailModel {
  medicationID: number;
  medicationName: string;
  barCode: string;
  description: string;
  cost: number;
  quantity: number;
  expiryDate: string; // ISO date string (YYYY-MM-DD)
}

