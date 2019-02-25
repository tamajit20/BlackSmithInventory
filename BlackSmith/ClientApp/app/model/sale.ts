import { ModelBase } from "./modelbase";
import { Customer } from "./customer";
import { Product } from "./product";

export class Sale extends ModelBase {
    fK_CustomerId: number
    note: string;
    totalTax: number;
    cGSTRate: number;
    sGSTRate: number;
    discount: number;
    total: number;
    finalTotal: number;
    billDate: Date;
    date: string;
    saleDetails: SaleDetail[];
    paymentDetails: SalePayment[];
}

export class SaleDetail extends ModelBase {
    saleDetailNo: number;
    fK_ProductId: number;
    fK_CustomerId: number;
    quantity: number;
    price: number;
    total: number;
}

export class SalePayment extends ModelBase {
    fK_SaleId: number;
    amount: number;
    note: number;
}

export class SaleLoader extends ModelBase {
    customers: Customer[];
    products: Product[];
}