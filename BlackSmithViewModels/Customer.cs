using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Customer : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string GSTIN { get; set; }
        public string PAN { get; set; }
        public virtual List<Sale> Sales { get; set; }
    }
}
