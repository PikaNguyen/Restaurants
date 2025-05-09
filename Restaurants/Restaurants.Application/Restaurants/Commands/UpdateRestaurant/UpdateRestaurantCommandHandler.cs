using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    /// <summary>
    /// Handler for updating an existing restaurant
    /// </summary>
    public class UpdateRestaurantCommandHandler(
        ILogger<UpdateRestaurantCommandHandler> logger,
        IMapper mapper,
        IRestaurantAuthorizeService restaurantAuthorizeService,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand, bool>
    {
        /// <summary>
        /// Handles the update of an existing restaurant
        /// </summary>
        /// <param name="request">The command containing restaurant update details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the update was successful, false otherwise</returns>
        /// <remarks>
        /// This method:
        /// 1. Retrieves the restaurant from the repository
        /// 2. Checks if the current user is authorized to update the restaurant
        /// 3. Maps the update command to the restaurant entity
        /// 4. Saves the changes to the repository
        /// </remarks>
        /// <exception cref="ForbidException">Thrown when the user is not authorized to update the restaurant</exception>
        public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isUpdate = false;
                logger.LogInformation($"Getting restaurant with id: {request.Id} form db");
                var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);
                if (restaurant != null)
                {
                    logger.LogInformation($"Update restaurant with id: {request.Id} form db");
                    if (!restaurantAuthorizeService.Authorize(restaurant, ResourceOperation.Update))
                    {
                        throw new ForbidException();
                    }

                    mapper.Map(request, restaurant);
                    /*restaurant.Name = request.Name;
                    restaurant.Description = request.Description;
                    restaurant.HasDelivery = request.HasDelivery;*/
                    await restaurantsRepository.SaveChangesAsync();
                    isUpdate = true;
                }

                return isUpdate;
            }
            catch (Exception ex) 
            {
                logger.LogError($"Error: {ex.ToString()}");
                return false;
            }
        }
    }
}
