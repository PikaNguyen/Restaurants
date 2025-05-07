using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public interface IRestaurantAuthorizeService
    {
        bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation);
    }
}