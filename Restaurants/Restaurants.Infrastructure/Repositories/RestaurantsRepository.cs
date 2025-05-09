using Azure.Core;
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

        /// <summary>
        /// Creates a new restaurant in the database
        /// </summary>
        /// <param name="request">The restaurant entity to create</param>
        /// <returns>The ID of the newly created restaurant</returns>
        public async Task<int> CreateNewRestaurant(Restaurant request)
        {
            await _dbContext.Restaurants.AddAsync(request);
            var id = await _dbContext.SaveChangesAsync();
            return request.Id;
        }

        /// <summary>
        /// Deletes a restaurant from the database
        /// </summary>
        /// <param name="restaurant">The restaurant entity to delete</param>
        /// <returns>True if the restaurant was successfully deleted, false otherwise</returns>
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

        /// <summary>
        /// Retrieves all restaurants from the database
        /// </summary>
        /// <returns>A collection of all restaurants</returns>
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            var restaurant = await _dbContext.Restaurants.ToListAsync();
            return restaurant;
        }

        /// <summary>
        /// Retrieves restaurants matching a search phrase with pagination
        /// </summary>
        /// <param name="searchPhrase">Optional search phrase to filter restaurants by name or description</param>
        /// <param name="pageSize">The number of restaurants to return per page</param>
        /// <param name="pageNumber">The page number to retrieve</param>
        /// <returns>A tuple containing the matching restaurants and the total count</returns>
        public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingRestaurantsAsync(string searchPhrase, int pageSize, int pageNumber)
        {
            var searchPhraseLower = searchPhrase?.ToLower();
            var query = _dbContext.Restaurants
                .Where(r => searchPhraseLower == null   || (r.Name.ToLower().Contains(searchPhraseLower)
                                                        || r.Description.ToLower().Contains(searchPhraseLower)));

            var totalCount = await query.CountAsync();

            var restaurant = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return (restaurant, totalCount);
        }

        /// <summary>
        /// Retrieves a specific restaurant by its ID
        /// </summary>
        /// <param name="id">The ID of the restaurant to retrieve</param>
        /// <returns>The restaurant with the specified ID, or null if not found</returns>
        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
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

        /// <summary>
        /// Saves all pending changes to the database
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
