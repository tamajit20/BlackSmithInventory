import { ModelBase } from "./modelbase";
import { InventoryItem } from "./inventoryitem";
import { Suplier } from "./suplier";

export class Purchase extends ModelBase {
    note: string;
    cgstRate: number;
    cgstTax: number;
    sgstTax: number;
    sgstRate: number;
    discount: number;
    total: number;
    finalTotal: number;
    totalPaid: number;
    purchaseDate: string;
    finalTotalInWords: string;
    purchaseDetails: PurchaseDetail[];
    paymentDetails: PurchasePayment[];
    isGenerated: boolean = false;
    due: number;
    paymentTerm: string;
    dispatchThru: string;
    purchaseId: string;
    fK_SuplierId: number;
    suplier: Suplier;
}

export class PurchaseDetail extends ModelBase {
    purchaseDetailNo: number;
    fK_InventoryItemId: number;
    fK_PurchaseId: number;
    quantity: number;
    price: number;
    total: number;
    item: InventoryItem;
    suplier: Suplier;
}

export class PurchasePayment extends ModelBase {
    fK_PurchaseId: number;
    amount: number;
    note: number;
    billId: string;
    purchase: Purchase;
    paymentDate: string;
}

export class PurchaseList extends ModelBase {
    purchases: Purchase[] = [];
}
export class PurchaseLoader extends ModelBase {
    supliers: Suplier[];
    items: InventoryItem[];
}