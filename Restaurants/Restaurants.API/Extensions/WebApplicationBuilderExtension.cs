using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Persistance;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference  = new OpenApiReference {Type = ReferenceType.SecurityScheme,Id = "bearerAuth"}
            },
            []
        }
    });
        });

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

        builder.Services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantDBContext>();
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
            .ReadFrom.Configuration(context.Configuration);
        });
    }
}
