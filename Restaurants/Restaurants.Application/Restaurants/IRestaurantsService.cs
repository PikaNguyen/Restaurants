using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsService
    {
        Task<IEnumerable<RestaurantsDTO>> GetAllRestaurantsAsync();
        Task<RestaurantsDTO> GetRestaurantAsync(int id);
        Task<int> CreateNewRestaurant(RestaurantRequest request);
    }
}
