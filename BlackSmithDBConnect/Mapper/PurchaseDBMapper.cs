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
    public class PurchaseDBMapper : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CGSTRate);
            builder.Property(c => c.SGSTRate);
            builder.Property(c => c.CGSTTax);
            builder.Property(c => c.SGSTTax);
            builder.Property(c => c.Note);
            builder.Property(c => c.PurchaseDate);
            builder.Property(c => c.Discount);
            builder.Property(c => c.PurchaseId);
            builder.Property(c => c.Total);
            builder.Property(c => c.FinalTotal);
            builder.Property(c => c.DispatchThru);
            builder.Property(c => c.PaymentTerm);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);

            builder.Ignore(c=>c.FinalTotalInWords);
            builder.Ignore(c => c.TotalPaid);
            builder.Ignore(c => c.Due);

            builder.ToTable("tPurchase");
        }
    }

    public class PurchaseDetailDBMapper : IEntityTypeConfiguration<PurchaseDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Fk_InventoryItemId);
            builder.Property(c => c.FK_PurchaseId);
            builder.Property(c => c.Price);
            builder.Property(c => c.Quantity);
            builder.Property(c => c.FK_SuplierId);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);

            builder.Ignore(c => c.PurchaseDetailNo);
            builder.Ignore(c => c.Total);

            builder.HasOne(p => p.Purchase).WithMany(f => f.PurchaseDetails).HasForeignKey(k => k.FK_PurchaseId);
            builder.HasOne(p => p.Item).WithMany(f => f.PurchaseDetails).HasForeignKey(k => k.Fk_InventoryItemId);
            builder.HasOne(p => p.Suplier).WithMany(f => f.PurchaseDetails).HasForeignKey(k => k.FK_SuplierId);

            builder.ToTable("tPurchaseDetail");
        }
    }

    public class PurchasePaymentDBMapper : IEntityTypeConfiguration<PurchasePayment>
    {
        public void Configure(EntityTypeBuilder<PurchasePayment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FK_PurchaseId);
            builder.Property(c => c.Amount);
            builder.Property(c => c.Note);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);
            builder.Property(c => c.PaymentDate);

            builder.Ignore(c => c.BillId);

            builder.HasOne(p => p.Purchase).WithMany(f => f.PurchasePayments).HasForeignKey(k => k.FK_PurchaseId);

            builder.ToTable("tPurchasePayment");
        }
    }
}