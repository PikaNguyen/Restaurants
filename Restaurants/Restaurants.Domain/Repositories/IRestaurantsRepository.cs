using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurants();
        Task<Restaurant> GetRestaurantAsync(int id);
        Task<int> CreateNewRestaurant(Restaurant request);
        Task<bool> DeleteRestaurantById(Restaurant restaurant);
        Task SaveChangesAsync();
    }
}
