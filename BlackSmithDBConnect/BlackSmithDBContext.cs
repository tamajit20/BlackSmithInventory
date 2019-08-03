using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Microsoft.EntityFrameworkCore;
using static BlackSmithDBConnect.ProductionDBMapper;
using static BlackSmithDBConnect.ProductionDBMapper.ProductionInventoryItemDBMapper;

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
            modelBuilder.ApplyConfiguration(new ProductionDBMapper());
            modelBuilder.ApplyConfiguration(new ProductionInventoryItemDBMapper());
            modelBuilder.ApplyConfiguration(new ProductionProductDBMapper());
        }      
    }
}