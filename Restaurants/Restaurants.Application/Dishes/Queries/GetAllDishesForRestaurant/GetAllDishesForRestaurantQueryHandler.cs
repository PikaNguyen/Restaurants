using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant
{
    public class GetAllDishesForRestaurantQueryHandler(
        ILogger<GetAllDishesForRestaurantQueryHandler> logger,
        IMapper mapper,
        IDishesRepository dishesRepository,
        IRestaurantsRepository restaurantsRepository
        ) : IRequestHandler<GetAllDishesForRestaurantQuery, IEnumerable<DishDTO>>
    {
        public async Task<IEnumerable<DishDTO>> Handle(GetAllDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            var rest = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            if (rest != null)
            {
                var result = mapper.Map<IEnumerable<DishDTO>>(rest.Dishes);
                return result;
            }

            return Enumerable.Empty<DishDTO>();
        }
    }
}
