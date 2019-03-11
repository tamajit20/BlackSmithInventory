import { ModelBase } from "./modelbase";

export class SearchObject extends ModelBase {
    billIds: string[] = [];
    fromDate: string;
    toDate: string;
    customerIds: number[] = [];
    productIds: number[] = [];
}