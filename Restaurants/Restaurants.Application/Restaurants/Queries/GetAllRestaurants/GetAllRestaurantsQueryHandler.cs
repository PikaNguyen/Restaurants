using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantsDTO>>
    {
        public async Task<PagedResult<RestaurantsDTO>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var searchPhrase = request.SearchPhrase;
            logger.LogInformation("Getting all restaurants form db");
            if (searchPhrase != null)
            {
                var (restaurants, total) = await restaurantsRepository.GetAllMatchingRestaurantsAsync(searchPhrase, request.PageSize, request.PageNumber);
                var restaurantsDTO = mapper.Map<IEnumerable<RestaurantsDTO>>(restaurants);
                var result = new PagedResult<RestaurantsDTO>(restaurantsDTO,total,request.PageSize, request.PageNumber);
                return result;
            }
            else
            {
                var restaurants = await restaurantsRepository.GetAllRestaurantsAsync();
                var restaurantsDTO = mapper.Map<IEnumerable<RestaurantsDTO>>(restaurants);
                var result = new PagedResult<RestaurantsDTO>(restaurantsDTO);
                return result;
            }
        }
    }
}
