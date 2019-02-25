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
    public class SaleDBMapper : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FK_CustomerId);
            builder.Property(c => c.CGSTRate);
            builder.Property(c => c.SGSTRate);
            builder.Property(c => c.TotalTax);
            builder.Property(c => c.Note);
            builder.Property(c => c.Date);
            builder.Property(c => c.Discount);
            builder.Property(c => c.BillId);
            builder.Property(c => c.Total);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);

            builder.ToTable("tSale");
        }
    }

    public class SaleDetailDBMapper : IEntityTypeConfiguration<SaleDetail>
    {
        public void Configure(EntityTypeBuilder<SaleDetail> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FK_ProductId);
            builder.Property(c => c.FK_SaleId);
            builder.Property(c => c.Price);
            builder.Property(c => c.Quantity);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);

            builder.HasOne(p => p.Sale).WithMany(f => f.SaleDetails).HasForeignKey(k => k.FK_SaleId);

            builder.ToTable("tSaleDetail");
        }
    }
}