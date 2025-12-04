export interface MedicationListModel {
    medicationID: number;
    barCode: string;
    name: string;
    cost: number;
    quantity?: number;
}
