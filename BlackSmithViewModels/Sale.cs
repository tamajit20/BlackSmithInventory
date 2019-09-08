using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Sale : BaseModel
    {
        public virtual long FK_CustomerId { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal CGSTRate { get; set; }
        public virtual decimal CGSTTax { get; set; }
        public virtual decimal SGSTRate { get; set; }
        public virtual decimal SGSTTax { get; set; }
        public virtual string Note { get; set; }
        public virtual string BillId { get; set; }
        public virtual DateTime BillDate { get; set; }
        public virtual decimal Total { get; set; }
        public virtual decimal FinalTotal { get; set; }
        public virtual decimal RoundOffTotal { get; set; }
        public virtual string FinalTotalInWords { get; set; }
        public string PaymentTerm { get; set; }
        public string DispatchThru { get; set; }

        public virtual List<SaleDetail> SaleDetails { get; set; }
        public virtual List<SalePayment> SalePayments { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual decimal TotalPaid { get; set; }
        public virtual decimal Due { get; set; }
    }

    public class SaleDetail : BaseModel
    {
        public virtual long FK_SaleId { get; set; }
        public virtual long FK_ProductId { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual Sale Sale { get; set; }
        public virtual long SaleDetailNo { get; set; }
        public virtual decimal Total { get; set; }
        public virtual Product Product { get; set; }
        public virtual decimal AvailableQuantity { get; set; }
    }

    public class SalePayment : BaseModel
    {
        public virtual DateTime PaymentDate { get; set; }
        public virtual long FK_SaleId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Note { get; set; }
        public virtual string BillId { get; set; }
        public virtual Sale Sale { get; set; }
    }

    public class SaleList : BaseModel
    {
        public virtual List<Sale> Sales { get; set; }
    }
}
