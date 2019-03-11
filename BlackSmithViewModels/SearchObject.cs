using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SearchObject : BaseModel
    {
        public List<string> BillIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> ProductIds { get; set; }
    }
}
