using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
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
        IDishesRepository repositoryDish,
        IRestaurantsRepository restaurantsRepository
        ) : IRequestHandler<CreateDishCommand>
    {
        public async Task Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId);
            var mapRequest = mapper.Map<Dish>(request);
            if (restaurant != null) {
                var dish = await repositoryDish.CreateDishes(mapRequest);
            }
        }
    }
}
