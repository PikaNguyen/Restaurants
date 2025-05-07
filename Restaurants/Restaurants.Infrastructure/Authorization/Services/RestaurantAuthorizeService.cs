using Microsoft.Extensions.Logging;
using Restaurants.Application.User;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public class RestaurantAuthorizeService(ILogger<RestaurantAuthorizeService> logger,
        IUserContext userContext) : IRestaurantAuthorizeService
    {
        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var currentUser = userContext.GetCurrentUser();

            logger.LogInformation("Authorizing user {UserEmail} to {Operation} for restaurant {RestaurantName}",
                currentUser.Email, resourceOperation, restaurant.Name);

            if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.Read)
            {
                logger.LogInformation("Create/read operation - successful authorization");
                return true;
            }

            if (resourceOperation == ResourceOperation.Delete && currentUser.IsInRole(UserRoles.Admin))
            {
                logger.LogInformation("Admin user, delete operation - successful authorization");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update)
                && currentUser.Id == restaurant.OwnerId)
            {
                logger.LogInformation("Restaurant owner - successful authorization");
                return true;
            }

            return false;
        }
    }
}
