using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Product : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Image { get; set; }
        public virtual double Price { get; set; }
        public virtual List<SaleDetail> SaleDetails { get; set; }
        public virtual double Availibility { get; set; }
    }
}
