using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant
{
    public class GetDishByIdForRestaurantQueryHandler(
        ILogger<GetDishByIdForRestaurantQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository
        ) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDTO>
    {
        public async Task<DishDTO> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Get dish {DishId}, for restaurant with id: {RestaurantId}",
                request.DishId,request.RestaurantId);
            var rest = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (rest != null)
            {
                var dish = rest.Dishes.FirstOrDefault(d => d.Id == request.DishId);
                var result = mapper.Map<DishDTO>(dish);
                return result;
            }

            return null;
        }
    }
}
