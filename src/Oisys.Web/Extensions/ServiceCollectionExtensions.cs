using Microsoft.Extensions.DependencyInjection;
using OisysNew.Helpers;
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
                .AddTransient<IInventoryService, InventoryService>();
        }
    }
}
