using Microsoft.Extensions.DependencyInjection;
using ViewModels;
using BlackSmithCore;

namespace BlackSmith.StartUpObjects
{
    public class RegisterRepositories
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IOperation<User>, Operation<User>>();
            services.AddScoped<IOperation<Customer>, Operation<Customer>>();
            services.AddScoped<IOperation<Suplier>, Operation<Suplier>>();
            services.AddScoped<IOperation<InventoryItem>, Operation<InventoryItem>>();
            services.AddScoped<IOperation<Product>, Operation<Product>>();
            services.AddScoped<IOperation<Sale>, Operation<Sale>>();
            services.AddScoped<IOperation<SaleDetail>, Operation<SaleDetail>>();
            services.AddScoped<IOperation<SalePayment>, Operation<SalePayment>>();
            services.AddScoped<IOperation<Purchase>, Operation<Purchase>>();
            services.AddScoped<IOperation<PurchaseDetail>, Operation<PurchaseDetail>>();
            services.AddScoped<IOperation<PurchasePayment>, Operation<PurchasePayment>>();
            services.AddScoped<IOperation<Production>, Operation<Production>>();
            services.AddScoped<IOperation<ProductionInventoryItem>, Operation<ProductionInventoryItem>>();
            services.AddScoped<IOperation<ProductionProduct>, Operation<ProductionProduct>>();
        }
    }
}
