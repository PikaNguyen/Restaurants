using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantsDTO>>
    {
        public async Task<IEnumerable<RestaurantsDTO>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all restaurants form db");
            var restaurants = await restaurantsRepository.GetAllRestaurants();
            //var restaurantsDTO = restaurants.Select(RestaurantsDTO.FromEntity).ToList();
            var restaurantsDTO = mapper.Map<IEnumerable<RestaurantsDTO>>(restaurants);

            return restaurantsDTO;
        }
    }
}
