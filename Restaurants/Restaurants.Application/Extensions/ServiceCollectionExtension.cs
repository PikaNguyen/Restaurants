using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services) {

            var assembly = typeof(ServiceCollectionExtension).Assembly;
            services.AddScoped<IRestaurantsService, RestaurantsService>();
            services.AddAutoMapper(assembly);
            //Add service for FluentValidation
            services.AddValidatorsFromAssembly(assembly)
                .AddFluentValidationAutoValidation();
        }

    }
}
