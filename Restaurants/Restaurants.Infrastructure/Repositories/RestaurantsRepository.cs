using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;

namespace Restaurants.Infrastructure.Repositories
{
    public class RestaurantsRepository : IRestaurantsRepository
    {
        private readonly RestaurantDBContext _dbContext;
        public RestaurantsRepository(RestaurantDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<IEnumerable<Restaurant>> GetAllRestaurants()
        {
            var restaurant = await _dbContext.Restaurants.ToListAsync();
            return restaurant;
        }
    }
}
