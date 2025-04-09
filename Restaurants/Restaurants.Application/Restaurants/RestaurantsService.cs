using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants
{
    public class RestaurantsService(IRestaurantsRepository restaurantRepository,
        ILogger<RestaurantsService> logger, IMapper mapper) : IRestaurantsService
    {
        public async Task<int> CreateNewRestaurant(RestaurantRequest request)
        {
            logger.LogInformation("Creating a new restaurant ");
            var mapRequest = mapper.Map<Restaurant>(request);
            var restaurant = await restaurantRepository.CreateNewRestaurant(mapRequest);

            return restaurant;
        }

        public async Task<IEnumerable<RestaurantsDTO>> GetAllRestaurantsAsync()
        {
            logger.LogInformation("Getting all restaurants form db");
            var restaurants = await restaurantRepository.GetAllRestaurants();
            //var restaurantsDTO = restaurants.Select(RestaurantsDTO.FromEntity).ToList();
            var restaurantsDTO = mapper.Map<IEnumerable<RestaurantsDTO>>(restaurants);

            return restaurantsDTO;
        }

        public async Task<RestaurantsDTO> GetRestaurantAsync(int id)
        {
            logger.LogInformation($"Getting restaurant with id: {id} form db");
            var restaurant = await restaurantRepository.GetRestaurantAsync(id);
            //var restaurantsDTO = RestaurantsDTO.FromEntity(restaurant);
            var restaurantsDTO = mapper.Map<RestaurantsDTO>(restaurant);
            return restaurantsDTO;
        }
    }
}
