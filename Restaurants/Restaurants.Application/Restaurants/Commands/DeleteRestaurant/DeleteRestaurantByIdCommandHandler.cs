using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantByIdCommandHandler(
        ILogger<DeleteRestaurantByIdCommandHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantByIdCommand, bool>
    {
        public async Task<bool> Handle(DeleteRestaurantByIdCommand request, CancellationToken cancellationToken)
        {
            var isDeleted = false;
            logger.LogInformation("Getting restaurant with id: @{RestaurantId} form db", request.Id);
            var restaurant = await restaurantsRepository.GetRestaurantAsync(request.Id);
            if (restaurant != null) 
            {
                logger.LogInformation($"Deleting restaurant with id: {request.Id} form db");
                isDeleted = await restaurantsRepository.DeleteRestaurantById(restaurant);
            }

            return isDeleted ;
        }
    }
}
