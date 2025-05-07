using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Services;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantByIdCommandHandler(
        ILogger<DeleteRestaurantByIdCommandHandler> logger,
        IRestaurantAuthorizeService restaurantAuthorizeService,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantByIdCommand, bool>
    {
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

            return isDeleted ;
        }
    }
}
