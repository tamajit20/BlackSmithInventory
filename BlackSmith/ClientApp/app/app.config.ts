import { Headers, RequestOptions } from '@angular/http';

export class AppConfig {
    public static API_ENDPOINT = "http://localhost:54885/api/";
    public static REQUEST_HEADER = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });

    public static USER_VALIDATE = "User/Validate";

    public static CUSTOMER_SAVE = "Customer/Save";
    public static CUSTOMER_GETALL = "Customer/GetAll";
    public static CUSTOMER_DELETE = "Customer/Delete";

    public static PRODUCT_SAVE = "Product/Save";
    public static PRODUCT_GETALL = "Product/GetAll";
    public static PRODUCT_DELETE = "Product/delete";

    public static PRODUCTION_SAVE = "Production/Save";
    public static PRODUCTION_GETALL = "Production/GetAll";
    public static PRODUCTION_DELETE = "Production/delete";

    public static SALE_DOWNLOAD = "Sale/DownloadBill";
    public static SALE_SAVE = "Sale/Save";
    public static SALE_GETALL = "Sale/GetAll";
    public static SALE_DELETE = "Sale/delete";
    public static SALE_GETONE = "Sale/GetOne";
    public static SALE_PAYMENT = "Sale/Payment";
    public static SALE_LIST_ONSCREEN = "Sale/GetSaleListOnScreen";
    public static SALE_LIST_ONEXCEL = "Sale/GetSaleListOnExcel";

    
    public static SUPLIER_SAVE = "Suplier/Save";
    public static SUPLIER_GETALL = "Suplier/GetAll";
    public static SUPLIER_DELETE = "Suplier/Delete";

    public static INVENTORYITEM_SAVE = "InventoryItem/Save";
    public static INVENTORYITEM_GETALL = "InventoryItem/GetAll";
    public static INVENTORYITEM_DELETE = "InventoryItem/Delete";

    public static PURCHASE_SAVE = "Purchase/Save";
    public static PURCHASE_GETALL = "Purchase/GetAll";
    public static PURCHASE_DELETE = "Purchase/delete";
    public static PURCHASE_LIST_ONSCREEN = "Purchase/GetPurchaseListOnScreen";
    public static PURCHASE_LIST_ONEXCEL = "Purchase/GetPurchaseListOnExcel";
    public static PURCHASE_DOWNLOAD = "Purchase/DownloadBill";
    public static PURCHASE_PAYMENT = "Purchase/Payment";

}