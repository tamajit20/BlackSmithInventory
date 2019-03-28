using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BlackSmithDBConnect
{
    public class BlackSmithDBContext : DbContext
    {
        public static string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerDBMapper());
            modelBuilder.ApplyConfiguration(new SuplierDBMapper());
            modelBuilder.ApplyConfiguration(new ProductDBMapper());
            modelBuilder.ApplyConfiguration(new InventoryItemDBMapper());
            modelBuilder.ApplyConfiguration(new SaleDetailDBMapper());
            modelBuilder.ApplyConfiguration(new SaleDBMapper());
            modelBuilder.ApplyConfiguration(new SalePaymentDBMapper());
            modelBuilder.ApplyConfiguration(new PurchaseDBMapper());
            modelBuilder.ApplyConfiguration(new PurchaseDetailDBMapper());
            modelBuilder.ApplyConfiguration(new PurchasePaymentDBMapper());
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Suplier> Suplier { get; set; }
        public virtual DbSet<InventoryItem> InventoryItem { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<SaleDetail> SaleDetail { get; set; }
        public virtual DbSet<SalePayment> SalePayment { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<PurchaseDetail> PurchaseDetail { get; set; }
        public virtual DbSet<PurchasePayment> PurchasePayment { get; set; }
    }
}