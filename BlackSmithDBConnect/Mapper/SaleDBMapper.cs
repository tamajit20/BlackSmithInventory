﻿using Microsoft.EntityFrameworkCore;
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
            builder.Property(c => c.CGSTTax);
            builder.Property(c => c.SGSTTax);
            builder.Property(c => c.Note);
            builder.Property(c => c.BillDate);
            builder.Property(c => c.Discount);
            builder.Property(c => c.BillId);
            builder.Property(c => c.Total);
            builder.Property(c => c.FinalTotal);
            builder.Property(c => c.RoundOffTotal);
            builder.Property(c => c.DispatchThru);
            builder.Property(c => c.PaymentTerm);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);

            builder.Ignore(c=>c.FinalTotalInWords);
            builder.Ignore(c => c.TotalPaid);
            builder.Ignore(c => c.Due);

            builder.HasOne(p => p.Customer).WithMany(f => f.Sales).HasForeignKey(k => k.FK_CustomerId);

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

            builder.Ignore(c => c.SaleDetailNo);
            builder.Ignore(c => c.Total);
            builder.Ignore(c => c.AvailableQuantity);

            builder.HasOne(p => p.Sale).WithMany(f => f.SaleDetails).HasForeignKey(k => k.FK_SaleId);
            builder.HasOne(p => p.Product).WithMany(f => f.SaleDetails).HasForeignKey(k => k.FK_ProductId);

            builder.ToTable("tSaleDetail");
        }
    }

    public class SalePaymentDBMapper : IEntityTypeConfiguration<SalePayment>
    {
        public void Configure(EntityTypeBuilder<SalePayment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FK_SaleId);
            builder.Property(c => c.Amount);
            builder.Property(c => c.Note);

            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);
            builder.Property(c => c.PaymentDate);

            builder.Ignore(c => c.BillId);

            builder.HasOne(p => p.Sale).WithMany(f => f.SalePayments).HasForeignKey(k => k.FK_SaleId);

            builder.ToTable("tSalePayment");
        }
    }
}