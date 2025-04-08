using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants
{
    public class RestaurantsService(IRestaurantsRepository restaurantRepository,
        ILogger<RestaurantsService> logger) : IRestaurantsService
    {
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            logger.LogInformation("Getting all restaurants form db");
            var restaurants = await restaurantRepository.GetAllRestaurants();
            return restaurants;
        }

    }
}
