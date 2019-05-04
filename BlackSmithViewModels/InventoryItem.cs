using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class InventoryItem : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual double Availibility { get; set; }
        public virtual List<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
