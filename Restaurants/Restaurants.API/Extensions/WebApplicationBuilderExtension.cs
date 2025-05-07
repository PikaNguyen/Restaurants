using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Constants;
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
            //Config custom user claims: More info nationality and dateOfBirth
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantDBContext>();

        // Add policy to check User who defined nationality or not
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(ConstantAuthentication.HasNationality, builder => builder.RequireClaim(ConstantAuthentication.Nationality, "VietNamese"))
            .AddPolicy(ConstantAuthentication.AtLeast,
            builder => builder.AddRequirements(new MinimumAgeRequirement(18)))
            .AddPolicy(ConstantAuthentication.Created2Restaurants,
            builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));

        builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
            .ReadFrom.Configuration(context.Configuration);
        });
    }
}
