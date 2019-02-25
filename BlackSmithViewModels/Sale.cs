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
        public virtual double SGSTRate { get; set; }
        public virtual double TotalTax { get; set; }
        public virtual string Note { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string BillId { get; set; }
        public virtual double Total { get; set; }

        public virtual List<SaleDetail> SaleDetails { get; set; }
    }

    public class SaleDetail : BaseModel
    {
        public virtual long FK_SaleId { get; set; }
        public virtual long FK_ProductId { get; set; }
        public virtual double Price { get; set; }
        public virtual double Quantity { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
