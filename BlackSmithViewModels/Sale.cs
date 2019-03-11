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
        public virtual double Discount { get; set; }
        public virtual double CGSTRate { get; set; }
        public virtual double CGSTTax { get; set; }
        public virtual double SGSTRate { get; set; }
        public virtual double SGSTTax { get; set; }
        public virtual string Note { get; set; }
        public virtual string BillId { get; set; }
        public virtual DateTime BillDate { get; set; }
        public virtual double Total { get; set; }
        public virtual double FinalTotal { get; set; }
        public virtual string FinalTotalInWords { get; set; }

        public virtual List<SaleDetail> SaleDetails { get; set; }
        public virtual List<SalePayment> SalePayments { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual double TotalPaid { get; set; }
        public virtual double Due { get; set; }
    }

    public class SaleDetail : BaseModel
    {
        public virtual long FK_SaleId { get; set; }
        public virtual long FK_ProductId { get; set; }
        public virtual double Price { get; set; }
        public virtual double Quantity { get; set; }
        public virtual Sale Sale { get; set; }
        public virtual long SaleDetailNo { get; set; }
        public virtual double Total { get; set; }
        public virtual Product Product { get; set; }

    }

    public class SalePayment : BaseModel
    {
        public virtual DateTime PaymentDate { get; set; }
        public virtual long FK_SaleId { get; set; }
        public virtual double Amount { get; set; }
        public virtual string Note { get; set; }
        public virtual string BillId { get; set; }
        public virtual Sale Sale { get; set; }
    }

    public class SaleList : BaseModel
    {
        public virtual List<Sale> Sales { get; set; }
    }
}
