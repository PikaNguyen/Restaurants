using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistance;

namespace Restaurants.Infrastructure.Repositories
{
    public class DishesRepository : IDishesRepository
    {
        private readonly RestaurantDBContext _dbContext;
        public DishesRepository(RestaurantDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<int> CreateDishes(Dish dish)
        {
            _dbContext.Dishes.Add(dish);
            await _dbContext.SaveChangesAsync();

            return dish.Id; 
        }
    }
}
