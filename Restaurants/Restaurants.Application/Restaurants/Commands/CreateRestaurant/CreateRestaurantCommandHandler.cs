using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.User;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    /// <summary>
    /// Handler for creating a new restaurant
    /// </summary>
    public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper,
        IUserContext userContext,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateRestaurantCommand, int>
    {
        /// <summary>
        /// Handles the creation of a new restaurant
        /// </summary>
        /// <param name="request">The command containing restaurant creation details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The ID of the newly created restaurant</returns>
        /// <remarks>
        /// This method:
        /// 1. Gets the current user from the context
        /// 2. Maps the command to a Restaurant entity
        /// 3. Sets the owner ID to the current user
        /// 4. Creates the restaurant in the repository
        /// </remarks>
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            logger.LogInformation("{UserEmail} [{UserId}] is creating a new restaurant {@Restaurant} ", currentUser.Email, currentUser.Id, request);
            var mapRequest = mapper.Map<Restaurant>(request);
            mapRequest.OwnerId = currentUser.Id;
            var restaurant = await restaurantsRepository.CreateNewRestaurant(mapRequest);

            return restaurant;
        }
    }
}
