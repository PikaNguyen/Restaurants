using Restaurants.Infrastructure.Persistance;
using Restaurants.Domain.Entities;

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
        }
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
