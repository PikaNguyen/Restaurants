using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.User;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    /// <summary>
    /// Authorization handler that checks if a user has created a minimum number of restaurants
    /// </summary>
    /// <remarks>
    /// This handler is used to implement a policy that restricts access based on the number
    /// of restaurants a user has created. This can be used to implement features that are
    /// only available to users who have demonstrated active participation in the platform.
    /// </remarks>
    public class CreatedMultipleRestaurantsRequirementHandler(
        IRestaurantsRepository restaurantsRepository,
        IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
    {
        /// <summary>
        /// Handles the authorization requirement by checking the number of restaurants created by the user
        /// </summary>
        /// <param name="context">The authorization context containing the user's claims</param>
        /// <param name="requirement">The requirement specifying the minimum number of restaurants needed</param>
        /// <returns>A task that completes when the handler has made its determination</returns>
        /// <remarks>
        /// The handler will:
        /// 1. Get the current user from the context
        /// 2. Retrieve all restaurants from the repository
        /// 3. Count how many restaurants are owned by the current user
        /// 4. Succeed if the count exceeds the minimum requirement, fail otherwise
        /// </remarks>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            CreatedMultipleRestaurantsRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();

            var restaurants = await restaurantsRepository.GetAllRestaurantsAsync();

            var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);

            if (userRestaurantsCreated > requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
