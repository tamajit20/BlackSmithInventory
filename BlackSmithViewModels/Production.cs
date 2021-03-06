﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Production : BaseModel
    {
        public virtual DateTime Date { get; set; }
        public virtual string Note { get; set; }
        public virtual List<ProductionInventoryItem> ProductionInventoryItems { get; set; }
        public virtual List<ProductionProduct> ProductionProducts { get; set; }

    }

    public class ProductionInventoryItem : BaseModel
    {
        public virtual long FK_ProductionId { get; set; }
        public virtual long FK_InventoryItemId { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual Production Production { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }

        public virtual int DetailNo { get; set; }
    }

    public class ProductionProduct : BaseModel
    {
        public virtual long FK_ProductionId { get; set; }
        public virtual long FK_ProductId { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual Production Production { get; set; }
        public virtual Product Product { get; set; }
        public virtual int DetailNo { get; set; }

    }

    public class ProductionList : BaseModel
    {
        public virtual List<Production> Productions { get; set; }
    }
}