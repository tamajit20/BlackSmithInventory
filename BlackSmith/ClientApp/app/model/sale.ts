﻿import { ModelBase } from "./modelbase";
import { Customer } from "./customer";
import { Product } from "./product";

export class Sale extends ModelBase {
    fK_CustomerId: number;
    billId: string;
    note: string;
    cgstRate: number;
    cgstTax: number;
    sgstTax: number;
    sgstRate: number;
    discount: number;
    total: number;
    finalTotal: number;
    totalPaid: number;
    billDate: string;
    finalTotalInWords: string;
    saleDetails: SaleDetail[];
    paymentDetails: SalePayment[];
    isGenerated: boolean = false;
    customer: Customer;
    due: number;
    paymentTerm: string;
    dispatchThru: string;
    roundOffTotal: number;
}

export class SaleDetail extends ModelBase {
    saleDetailNo: number;
    fK_ProductId: number;
    FK_SaleId: number;
    quantity: number;
    price: number;
    total: number;
    product: Product;
    availableQuantity: number;
}

export class SalePayment extends ModelBase {
    fK_SaleId: number;
    amount: number;
    note: number;
    billId: string;
    sale: Sale;
    paymentDate: string;
}

export class SaleList extends ModelBase {
    sales: Sale[] = [];
}
export class SaleLoader extends ModelBase {
    customers: Customer[];
    products: Product[];
}