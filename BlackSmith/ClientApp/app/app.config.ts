import { Headers, RequestOptions } from '@angular/http';

export class AppConfig {
    public static API_ENDPOINT = "http://localhost:53305/api/";
    public static REQUEST_HEADER = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });

    public static USER_VALIDATE = "User/Validate";

    public static CUSTOMER_SAVE = "Customer/Save";
    public static CUSTOMER_GETALL = "Customer/GetAll";
    public static CUSTOMER_DELETE = "Customer/Delete";

    public static PRODUCT_SAVE = "Product/Save";
    public static PRODUCT_GETALL = "Product/GetAll";
    public static PRODUCT_DELETE = "Product/delete";
    
    public static SUPLIER_SAVE = "Suplier/Save";
    public static SUPLIER_GETALL = "Suplier/GetAll";
    public static SUPLIER_DELETE = "Suplier/Delete";

    public static INVENTORYITEM_SAVE = "InventoryItem/Save";
    public static INVENTORYITEM_GETALL = "InventoryItem/GetAll";
    public static INVENTORYITEM_DELETE = "InventoryItem/Delete";

    public static PURCHASE_SAVE = "Product/Save";
    public static PURCHASE_GETALL = "Product/GetAll";
    public static PURCHASE_DELETE = "Product/delete";

}