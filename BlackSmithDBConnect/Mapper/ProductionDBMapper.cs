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
    public class ProductionDBMapper : IEntityTypeConfiguration<Production>
    {
        public void Configure(EntityTypeBuilder<Production> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Date);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);

            builder.ToTable("tProduction");
        }

        public class ProductionInventoryItemDBMapper : IEntityTypeConfiguration<ProductionInventoryItem>
        {
            public void Configure(EntityTypeBuilder<ProductionInventoryItem> builder)
            {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.FK_InventoryItemId);
                builder.Property(c => c.FK_ProductionId);
                builder.Property(c => c.Quantity);

                builder.Property(c => c.CreatedOn);
                builder.Property(c => c.CreatedBy);
                builder.Property(c => c.ModifiedOn);
                builder.Property(c => c.ModifiedBy);

                builder.Ignore(c=>c.DetailNo);

                builder.HasOne(p => p.Production).WithMany(f => f.ProductionInventoryItems).HasForeignKey(k => k.FK_ProductionId);
                builder.HasOne(p => p.InventoryItem).WithMany(f => f.ProductionInventoryItems).HasForeignKey(k => k.FK_InventoryItemId);


                builder.ToTable("tProductionInventoryItem");
            }

            public class ProductionProductDBMapper : IEntityTypeConfiguration<ProductionProduct>
            {
                public void Configure(EntityTypeBuilder<ProductionProduct> builder)
                {
                    builder.HasKey(c => c.Id);
                    builder.Property(c => c.FK_ProductId);
                    builder.Property(c => c.FK_ProductionId);
                    builder.Property(c => c.Quantity);

                    builder.Property(c => c.CreatedOn);
                    builder.Property(c => c.CreatedBy);
                    builder.Property(c => c.ModifiedOn);
                    builder.Property(c => c.ModifiedBy);

                    builder.Ignore(c => c.DetailNo);

                    builder.HasOne(p => p.Production).WithMany(f => f.ProductionProducts).HasForeignKey(k => k.FK_ProductionId);
                    builder.HasOne(p => p.Product).WithMany(f => f.ProductionProducts).HasForeignKey(k => k.FK_ProductId);

                    builder.ToTable("tProductionProduct");
                }
            }
        }
    }
}