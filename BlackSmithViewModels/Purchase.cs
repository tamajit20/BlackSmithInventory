﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Purchase : BaseModel
    {
        public virtual decimal Discount { get; set; }
        public virtual decimal CGSTRate { get; set; }
        public virtual decimal CGSTTax { get; set; }
        public virtual decimal SGSTRate { get; set; }
        public virtual decimal SGSTTax { get; set; }
        public virtual string Note { get; set; }
        public virtual string PurchaseId { get; set; }
        public virtual DateTime PurchaseDate { get; set; }
        public virtual decimal Total { get; set; }
        public virtual decimal FinalTotal { get; set; }
        public virtual decimal RoundOffTotal { get; set; }
        public virtual string FinalTotalInWords { get; set; }
        public string PaymentTerm { get; set; }
        public string DispatchThru { get; set; }
        public virtual long FK_SuplierId { get; set; }

        public virtual List<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual List<PurchasePayment> PurchasePayments { get; set; }
        public virtual Suplier Suplier { get; set; }

        public virtual decimal TotalPaid { get; set; }
        public virtual decimal Due { get; set; }
    }

    public class PurchaseDetail : BaseModel
    {
        public virtual long FK_PurchaseId { get; set; }
        public virtual long Fk_InventoryItemId { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual long PurchaseDetailNo { get; set; }
        public virtual decimal Total { get; set; }
        public virtual InventoryItem Item { get; set; }
        public virtual decimal AvailableQuantity { get; set; }
    }

    public class PurchasePayment : BaseModel
    {
        public virtual DateTime PaymentDate { get; set; }
        public virtual long FK_PurchaseId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Note { get; set; }
        public virtual string BillId { get; set; }
        public virtual Purchase Purchase { get; set; }
    }

    public class PurchaseList : BaseModel
    {
        public virtual List<Purchase> Purchases { get; set; }
    }
}
