using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Services;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    /// <summary>
    /// Handler for deleting a restaurant
    /// </summary>
    public class DeleteRestaurantByIdCommandHandler(
        ILogger<DeleteRestaurantByIdCommandHandler> logger,
        IRestaurantAuthorizeService restaurantAuthorizeService,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantByIdCommand, bool>
    {
        /// <summary>
        /// Handles the deletion of a restaurant
        /// </summary>
        /// <param name="request">The command containing the restaurant ID to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the deletion was successful, false otherwise</returns>
        /// <remarks>
        /// This method:
        /// 1. Retrieves the restaurant from the repository
        /// 2. Checks if the current user is authorized to delete the restaurant
        /// 3. Deletes the restaurant from the repository
        /// </remarks>
        /// <exception cref="ForbidException">Thrown when the user is not authorized to delete the restaurant</exception>
        public async Task<bool> Handle(DeleteRestaurantByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = false;
            logger.LogInformation("Getting restaurant with id: @{RestaurantId} form db", request.Id);
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);
            if (restaurant != null) 
            {
                logger.LogInformation($"Deleting restaurant with id: {request.Id} form db");
                if(!restaurantAuthorizeService.Authorize(restaurant, ResourceOperation.Delete))
                {
                    throw new ForbidException();
                }

                isDeleted = await restaurantsRepository.DeleteRestaurantById(restaurant);
            }

            return isDeleted;
        }
    }
}
