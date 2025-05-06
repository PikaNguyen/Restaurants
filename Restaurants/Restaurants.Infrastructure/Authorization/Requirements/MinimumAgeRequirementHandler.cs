using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.User;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
    IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    /// <summary>
    /// Check requirement aboyt user's claims DateOfBirth 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement">Minimum age requirement</param>
    /// <returns></returns>
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
