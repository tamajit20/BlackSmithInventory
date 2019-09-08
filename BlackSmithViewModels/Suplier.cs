using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Suplier : BaseModel
    {
        virtual public string Name { get; set; }
        virtual public string Address { get; set; }
        virtual public string ContactNo { get; set; }
        virtual public string EmailId { get; set; }
        virtual public List<Purchase> Purchases { get; set; }
        virtual public string GSTIN { get; set; }
        virtual public string PAN { get; set; }
    }
}
