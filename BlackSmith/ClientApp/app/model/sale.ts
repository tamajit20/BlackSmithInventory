import { ModelBase } from "./modelbase";
import { Customer } from "./customer";
import { Product } from "./product";

export class Sale extends ModelBase {
    fK_CustomerId: number
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

export class SaleLoader extends ModelBase {
    customer: Customer[];
    product: Product[];
}