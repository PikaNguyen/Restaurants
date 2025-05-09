using Microsoft.Extensions.Logging;
using Restaurants.Application.User;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization.Services
{
    /// <summary>
    /// Service responsible for authorizing operations on restaurants based on user roles and ownership
    /// </summary>
    /// <remarks>
    /// This service implements the core authorization logic for restaurant operations,
    /// ensuring that users can only perform actions they are authorized for based on
    /// their role and relationship to the restaurant.
    /// </remarks>
    public class RestaurantAuthorizeService(ILogger<RestaurantAuthorizeService> logger,
        IUserContext userContext) : IRestaurantAuthorizeService
    {
        /// <summary>
        /// Authorizes a specific operation on a restaurant
        /// </summary>
        /// <param name="restaurant">The restaurant to authorize the operation for</param>
        /// <param name="resourceOperation">The type of operation to authorize (Create, Read, Update, Delete)</param>
        /// <returns>True if the operation is authorized, false otherwise</returns>
        /// <remarks>
        /// Authorization rules:
        /// 1. Create and Read operations are allowed for all authenticated users
        /// 2. Delete operations are allowed for admins and restaurant owners
        /// 3. Update operations are allowed only for restaurant owners
        /// </remarks>
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
