import { ModelBase } from "./modelbase";
import { InventoryItem } from "./inventoryitem";
import { Product } from "./product";

export class Production extends ModelBase {
    date: string;
    productionProducts: ProductionProduct[];
    productionInventoryItems: ProductionInventoryItem[];
    isGenerated: boolean = false;
}

export class ProductionProduct extends ModelBase {
    quantity: number;
    detailNo: number;
    fK_ProductionId: number;
    fK_ProductId: number;
    product: Product;
}

export class ProductionInventoryItem extends ModelBase {
    quantity: number;
    detailNo: number;
    fK_ProductionId: number;
    fK_InventoryItemId: number;
    inventoryitem: InventoryItem;
    availableQuantity: number;
}

export class ProductionLoader extends ModelBase {
    items: ProductionInventoryItem[];
    products: ProductionProduct[];
}

export class ProductionList extends ModelBase {
    productions: Production[] = [];
}