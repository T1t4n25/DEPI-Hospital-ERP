export interface InventoryDetailModel {
    medicationID: number;
    medicationName: string;
    barCode: string;
    description: string;
    cost: number;
    quantity: number;
    expiryDate: string;
}

export interface InventoryListModel {
    medicationID: number;
    medicationName: string;
    barCode: string;
    quantity: number;
    expiryDate: string;
}

export interface CreateInventoryModel {
    medicationID: number;
    quantity: number;
    expiryDate: string;
}

export interface UpdateInventoryModel extends CreateInventoryModel {
    medicationID: number;
}
