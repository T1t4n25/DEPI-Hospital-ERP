export interface InventoryListModel {
  medicationID: number;
  medicationName: string;
  barCode: string;
  quantity: number;
  expiryDate: string; // ISO date string (YYYY-MM-DD)
}

