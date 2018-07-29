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
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Suplier> Suplier { get; set; }
        public virtual DbSet<InventoryItem> InventoryItem { get; set; }

    }
}