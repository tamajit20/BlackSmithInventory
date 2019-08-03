using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SearchObject : BaseModel
    {
        public string BillIds { get; set; }
        public DateTime FromDate { get; set; } = DateTime.MinValue;
        public DateTime ToDate { get; set; } = DateTime.MaxValue;
        public List<long> CustomerIds { get; set; }
        public List<long> ProductIds { get; set; }
        public List<long> SaleIds { get; set; }
        public List<long> PurchaseIds { get; set; }
        public List<long> ProductionIds { get; set; }
    }
}
