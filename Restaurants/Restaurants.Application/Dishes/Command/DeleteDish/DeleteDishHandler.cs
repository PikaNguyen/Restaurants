using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.Application.Dishes.Command.DeleteDish
{
    /// <summary>
    /// Handler for deleting a dish from a restaurant
    /// </summary>
    public class DeleteDishHandler (ILogger<DeleteDishHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizeService restaurantAuthorizeService,
        IDishesRepository dishesRepository) : IRequestHandler<DeleteDishCommand, bool>
    {
        /// <summary>
        /// Handles the deletion of a dish from a restaurant
        /// </summary>
        /// <param name="request">The command containing the dish and restaurant IDs</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the deletion was successful, false otherwise</returns>
        /// <remarks>
        /// This method:
        /// 1. Retrieves the restaurant and dish from the repository
        /// 2. Checks if the current user is authorized to delete the dish
        /// 3. Deletes the dish from the repository
        /// </remarks>
        /// <exception cref="ForbidException">Thrown when the user is not authorized to delete the dish</exception>
        /// <exception cref="NotFoundException">Thrown when the restaurant or dish is not found</exception>
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
