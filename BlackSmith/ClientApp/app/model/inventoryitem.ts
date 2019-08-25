import { ModelBase } from "./modelbase";

export class InventoryItem extends ModelBase {
    name: string;
    description: string;
    availibility: number;
    ssn: string;
}