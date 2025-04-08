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

        public async Task<Restaurant> GetRestaurantAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(x=>x.Id ==id);
            if (restaurant == null)
            {
                return null;
            }

            return restaurant;
        }
    }
}
