using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OisysNew.Helpers;
using OisysNew.Helpers.Interfaces;
using OisysNew.Services;

namespace OisysNew.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IOisysDbContext>(provider => provider.GetService<OisysDbContext>());
            serviceCollection
                .AddTransient<IListHelpers, ListHelpers>()
                .AddTransient<IInventoryService, InventoryService>()
                .AddTransient<IEntityListHelpers, EntityListHelpers>()
                .AddTransient<IOrderService, OrderService>();
            serviceCollection.AddTransient(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        }
    }
}
