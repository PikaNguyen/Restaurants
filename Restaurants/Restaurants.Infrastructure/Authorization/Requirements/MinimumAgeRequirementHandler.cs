using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.User;

namespace Restaurants.Infrastructure.Authorization.Requirements;

/// <summary>
/// Handles the minimum age requirement for authorization.
/// </summary>
/// <param name="logger">Logger instance for logging authorization events</param>
/// <param name="userContext">User context to access current user information</param>
public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
    IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    /// <summary>
    /// Handles the minimum age requirement check for authorization.
    /// </summary>
    /// <param name="context">The authorization context containing the user and resource information</param>
    /// <param name="requirement">The minimum age requirement to be checked</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <remarks>
    /// This method checks if the current user meets the minimum age requirement by:
    /// 1. Verifying the user has a valid date of birth
    /// 2. Calculating if the user's age meets or exceeds the minimum age requirement
    /// </remarks>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email}, date of birth {DoB} - Handling MinimumAgeRequirement",
           currentUser.Email,
           currentUser.DateOfBirth);

        if(currentUser.DateOfBirth == null)
        {
            logger.LogWarning("User date of birh is null");
            context.Fail();
            return Task.CompletedTask;
        }

        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("Authorization succeded");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogError("User's age is not enough 18");
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
