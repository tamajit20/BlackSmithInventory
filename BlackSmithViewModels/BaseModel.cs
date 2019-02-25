using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BaseModel
    {
        public BaseModel()
        {
            CreatedOn = DateTime.Now;
            CreatedBy = 0;
            ModifiedOn = DateTime.Now;
            ModifiedBy = 0;
        }
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
