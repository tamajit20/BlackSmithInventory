using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Production : BaseModel
    {
        public virtual DateTime Date { get; set; }
        public virtual List<ProductionInventoryItem> ProductionInventoryItems { get; set; }
        public virtual List<ProductionProduct> ProductionProducts { get; set; }

    }

    public class ProductionInventoryItem : BaseModel
    {
        public virtual long FK_ProductionId { get; set; }
        public virtual long FK_InventoryItemId { get; set; }
        public virtual double Quantity { get; set; }
        public virtual Production Production { get; set; }
    }

    public class ProductionProduct : BaseModel
    {
        public virtual long FK_ProductionId { get; set; }
        public virtual long FK_ProductId { get; set; }
        public virtual double Quantity { get; set; }
        public virtual Production Production { get; set; }
    }
}