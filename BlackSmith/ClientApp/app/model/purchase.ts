import { ModelBase } from "./modelbase";
import { Suplier } from "./suplier";
import { InventoryItem } from "./inventoryitem";

export class Purchase extends ModelBase {
    fK_SuplierId: number;
    note: string;
    gSTAmount: number;
    gSTRate: number;

    detail: PurchaseDetail[];
    payment: PurchasePayment[];
}

export class PurchaseDetail extends ModelBase {
    fK_InventoryItemId: number;
    fK_PurchaseId: number;
    quantitiy: number;
    price: number;
    unit: string;
}

export class PurchasePayment extends ModelBase {
    fK_PurchaseId: number;
    amount: number;
    note: number;
}

export class PurchaseLoader extends ModelBase {
    suplier: Suplier[];
    inventoryItem: InventoryItem[];
}