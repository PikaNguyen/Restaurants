using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Create Dish
        /// </summary>
        /// <param name="dish"></param>
        /// <returns></returns>
        public async Task<int> CreateDishes(Dish dish)
        {
            _dbContext.Dishes.Add(dish);
            await _dbContext.SaveChangesAsync();

            return dish.Id; 
        }

        /// <summary>
        /// Delete dish
        /// </summary>
        /// <param name="dish"></param>
        /// <returns></returns>
        public async Task<bool> DeleteDishesAsync(Dish dish)
        {
            _dbContext.Dishes.Remove(dish);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
