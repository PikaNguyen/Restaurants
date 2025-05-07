using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<int> CreateNewRestaurant(Restaurant request);
        Task<bool> DeleteRestaurantById(Restaurant restaurant);
        Task SaveChangesAsync();
        Task<IEnumerable<Restaurant>> GetAllMatchingRestaurantsAsync(string searchPhrase);
    }
}
