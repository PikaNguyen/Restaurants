using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    /// <summary>
    /// Handler for retrieving all restaurants with optional filtering and pagination
    /// </summary>
    public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantsDTO>>
    {
        /// <summary>
        /// Handles the retrieval of all restaurants
        /// </summary>
        /// <param name="request">The query containing optional search and pagination parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A paged result containing the matching restaurants</returns>
        /// <remarks>
        /// This method:
        /// 1. If search parameters are provided, retrieves filtered and paginated results
        /// 2. Otherwise, retrieves all restaurants
        /// 3. Maps the results to DTOs
        /// 4. Returns a paged result containing the restaurants and total count
        /// </remarks>
        public async Task<PagedResult<RestaurantsDTO>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var searchPhrase = request.SearchPhrase;
            logger.LogInformation("Getting all restaurants form db");
            if (searchPhrase != null || request.PageSize != 0 || request.PageNumber !=0)
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
