using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Command.DeleteDish
{
    public class DeleteDishHandler (ILogger<DeleteDishHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository) : IRequestHandler<DeleteDishCommand, bool>
    {
        public async Task<bool> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = false;
            logger.LogInformation("Getting restaurant with id: @{RestaurantId} form db", request.RestaurantId);
            var restaurant = restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            var dish = restaurant.Result.Dishes.FirstOrDefault(x=> x.Id == request.Id);
            if (restaurant != null ) 
            {
                if (dish != null) {
                    isDeleted = await dishesRepository.DeleteDishesAsync(dish);
                    return isDeleted;
                }
            }

            return isDeleted;
        }
    }
}
