import { ModelBase } from "./modelbase";
import { InventoryItem } from "./inventoryitem";
import { Product } from "./product";

export class Production extends ModelBase {
    date: string;
    productionProducts: ProductionProduct[];
    productionItems: ProductionInventoryItem[];
    isGenerated: boolean = false;
}

export class ProductionProduct extends ModelBase {
    quantity: number;
    detailNo: number;
    fK_ProductionId: number;
    fK_ProductId: number;
}

export class ProductionInventoryItem extends ModelBase {
    quantity: number;
    detailNo: number;
    fK_ProductionId: number;
    fK_InventoryItemId: number;
}

export class ProductionLoader extends ModelBase {
    items: InventoryItem[];
    products: Product[];
}