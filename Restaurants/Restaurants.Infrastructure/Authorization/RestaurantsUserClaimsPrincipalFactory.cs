using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Constants;
using System.Security.Claims;

namespace Restaurants.Infrastructure.Authorization
{
    /// <summary>
    /// Factory for creating ClaimsPrincipal objects with custom claims for restaurant users
    /// </summary>
    /// <remarks>
    /// This factory extends the standard Identity claims with additional restaurant-specific claims,
    /// such as nationality and date of birth, which are used for authorization policies.
    /// </remarks>
    public class RestaurantsUserClaimsPrincipalFactory(UserManager<User> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<Domain.Entities.User, IdentityRole>(userManager, roleManager, options)
    {
        /// <summary>
        /// Creates a ClaimsPrincipal for a user with additional restaurant-specific claims
        /// </summary>
        /// <param name="user">The user to create claims for</param>
        /// <returns>A ClaimsPrincipal containing standard identity claims and custom restaurant claims</returns>
        /// <remarks>
        /// This method adds the following custom claims:
        /// 1. Nationality claim if the user has specified their nationality
        /// 2. Date of birth claim if the user has provided their birth date
        /// </remarks>
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var id = await GenerateClaimsAsync(user);

            if (user.Nationality != null) {
                id.AddClaim(new Claim(ConstantAuthentication.Nationality, user.Nationality));
            }

            if (user.DateOfBirth != null) 
            {
                id.AddClaim(new Claim(ConstantAuthentication.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
            }

            return new ClaimsPrincipal(id);
        }
    }
}
