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
    public class CustomerDBMapper : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name);
            builder.Property(c => c.Address);
            builder.Property(c => c.EmailId);
            builder.Property(c => c.CreatedOn);
            builder.Property(c => c.CreatedBy);
            builder.Property(c => c.ModifiedOn);
            builder.Property(c => c.ModifiedBy);
            builder.Property(c => c.ContactNo);

            builder.ToTable("tCustomer");
        }
    }
}