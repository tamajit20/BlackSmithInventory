import { ModelBase } from "./modelbase";
import { Customer } from "./customer";
import { Product } from "./product";

export class Sale extends ModelBase {
    fK_CustomerId: number
    note: string;
    cgstRate: number;
    cgstTax: number;
    sgstTax: number;
    sgstRate: number;
    discount: number;
    total: number;
    finalTotal: number;
    billDate: string;
    finalTotalInWords: string;
    saleDetails: SaleDetail[];
    paymentDetails: SalePayment[];
    isGenerated: boolean = false;
}

export class SaleDetail extends ModelBase {
    saleDetailNo: number;
    fK_ProductId: number;
    FK_SaleId: number;
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