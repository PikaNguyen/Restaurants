using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Command
{
    public class CreateDishCommandHandler (
        ILogger<CreateDishCommandHandler> logger,
        IMapper mapper,
        IRestaurantAuthorizeService restaurantAuthorizeService,
        IDishesRepository repositoryDish,
        IRestaurantsRepository restaurantsRepository
        ) : IRequestHandler<CreateDishCommand>
    {
        public async Task Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting restaurant with id: @{RestaurantId} form db", request.RestaurantId);
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            var mapRequest = mapper.Map<Dish>(request);
            if (restaurant != null) {
                if (!restaurantAuthorizeService.Authorize(restaurant, ResourceOperation.Create))
                {
                    throw new ForbidException();
                }

                logger.LogInformation("Create new dish successfully. Restaurant with id: @{RestaurantId} form db", request.RestaurantId);
                var dish = await repositoryDish.CreateDishes(mapRequest);
            }
        }
    }
}
