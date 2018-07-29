using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BlackSmithDBConnect
{
    public class InventoryItemDBMapper : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name);
            builder.Property(c => c.Description);
            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);

            builder.ToTable("tInventoryItem");
        }
    }
}