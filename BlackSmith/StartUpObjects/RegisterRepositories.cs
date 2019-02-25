using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels;
using BlackSmithCore;

namespace BlackSmith.StartUpObjects
{
    public class RegisterRepositories
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IOperation<Customer>, Operation<Customer>>();
            services.AddScoped<IOperation<Suplier>, Operation<Suplier>>();
            services.AddScoped<IOperation<InventoryItem>, Operation<InventoryItem>>();
            services.AddScoped<IOperation<Product>, Operation<Product>>();
            services.AddScoped<IOperation<Sale>, Operation<Sale>>();
            services.AddScoped<IOperation<SaleDetail>, Operation<SaleDetail>>();
        }
    }
}
