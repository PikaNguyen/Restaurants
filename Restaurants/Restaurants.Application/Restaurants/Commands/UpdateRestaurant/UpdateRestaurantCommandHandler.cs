using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler(
        ILogger<UpdateRestaurantCommandHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand, bool>
    {
        public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isUpdate = false;
                logger.LogInformation($"Getting restaurant with id: {request.Id} form db");
                var restaurant = await restaurantsRepository.GetRestaurantAsync(request.Id);
                if (restaurant != null)
                {
                    logger.LogInformation($"Update restaurant with id: {request.Id} form db");
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
