using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler(ILogger<GetRestaurantByIdQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetRestaurantByIdQuery, RestaurantsDTO>
    {
        public async Task<RestaurantsDTO> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Getting restaurant with id: {request.Id} form db");
            var restaurant = await restaurantsRepository.GetRestaurantAsync(request.Id);
            //var restaurantsDTO = RestaurantsDTO.FromEntity(restaurant);
            var restaurantsDTO = mapper.Map<RestaurantsDTO>(restaurant);
            return restaurantsDTO;
        }
    }
}
