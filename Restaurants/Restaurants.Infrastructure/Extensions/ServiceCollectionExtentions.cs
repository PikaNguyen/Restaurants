using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Extensions
{
    public static class ServiceCollectionExtentions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<RestaurantDBContext>();
        }
    }
}
