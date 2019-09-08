using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InventoryItem : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string SSN { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Availibility { get; set; }
        public virtual List<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual List<ProductionInventoryItem> ProductionInventoryItems { get; set; }
    }
}
