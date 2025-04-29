using Restaurants.Infrastructure.Persistance;
using Restaurants.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;

namespace Restaurants.Infrastructure.Seeders;

public class RestaurantSeeder(RestaurantDBContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurant = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurant);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
            new (UserRoles.User){
                NormalizedName = UserRoles.User.ToUpper()
            },
            new (UserRoles.Owner){
                NormalizedName = UserRoles.Owner.ToUpper()
            },
            new (UserRoles.Admin){
                NormalizedName = UserRoles.Admin.ToUpper()
            },
            ];

        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new(){
                Name = "KFC",
                Category = "Fast food",
                Description = "KFC is an American fast food restaurant chain",
                ContactEmail = "KFC@gmail.com",
                HasDelivery = true,
                Dishes = [
                    new()
                    {
                        Name = "Hot chicken",
                        Description = "Chicken boiled fry ",
                        Price = 10.3M,
                    }
                    ],
                Address = new(){
                    City = "London",
                    Street = "Cork St 5",
                    PostalCode = "W2cn"
                }
            },
            new Restaurant(){
                Name = "McDonald Szewska",
                Category = "Fast food",
                Description = "McDonald Szewska incorporated on December 21, 1964",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address(){
                    City = "London",
                    Street = "Boots 1912",
                    PostalCode= "W1f 8sr"
                }
            }
            ];

        return restaurants;
    }
}
