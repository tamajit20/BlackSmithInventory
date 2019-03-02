using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual long Id { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual long CreatedBy { get; set; }
        public virtual DateTime ModifiedOn { get; set; }
        public virtual long ModifiedBy { get; set; }
        public virtual bool IsDeleted { get; set; }
        [NotMapped]
        public virtual string Msg { get; set; }
        [NotMapped]
        public virtual bool IsFailure { get; set; }

    }
}
