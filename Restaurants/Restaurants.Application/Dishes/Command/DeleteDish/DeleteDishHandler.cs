using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.Application.Dishes.Command.DeleteDish
{
    public class DeleteDishHandler (ILogger<DeleteDishHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizeService restaurantAuthorizeService,
        IDishesRepository dishesRepository) : IRequestHandler<DeleteDishCommand, bool>
    {
        public async Task<bool> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = false;
            logger.LogInformation("Getting restaurant with id: @{RestaurantId} form db", request.RestaurantId);
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(x=> x.Id == request.Id);
            if (restaurant != null ) 
            {
                if (!restaurantAuthorizeService.Authorize(restaurant, ResourceOperation.Delete))
                {
                    throw new ForbidException();
                }

                if (dish != null) {
                   
                    isDeleted = await dishesRepository.DeleteDishesAsync(dish);
                    return isDeleted;
                }
            }
            else
            {
                throw new NotFoundException($"Restaurant with {request.Id} doesn't exist");
            }

            return isDeleted;
        }
    }
}
