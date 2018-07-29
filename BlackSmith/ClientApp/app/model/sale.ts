import { ModelBase } from "./modelbase";

export class Sale extends ModelBase {
    fK_CustomerId: number;
    note: string;
    gSTAmount: number;
    gSTRate: number;

    detail: SaleDetail[];
    payment: SalePayment[];
}

export class SaleDetail extends ModelBase {
    fK_ProductId: number;
    fK_CustomerId: number;
    quantitiy: number;
    price: number;
}

export class SalePayment extends ModelBase {
    fK_SaleId: number;
    amount: number;
    note: number;
}