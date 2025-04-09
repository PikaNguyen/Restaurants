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

        public async Task<int> CreateNewRestaurant(Restaurant request)
        {
            await _dbContext.Restaurants.AddAsync(request);
            var id = await _dbContext.SaveChangesAsync();
            return request.Id;
        }

        public async Task<bool> DeleteRestaurantById(Restaurant restaurant)
        {
            var result = false;
            if (restaurant!=null)
            {
                _dbContext.Restaurants.Remove(restaurant);
                await _dbContext.SaveChangesAsync();
                result = true;
            }

            return result;
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

        public Task SaveChangesAsync()
            => _dbContext.SaveChangesAsync();
    }
}
