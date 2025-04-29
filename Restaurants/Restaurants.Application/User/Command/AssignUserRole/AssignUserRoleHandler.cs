using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Restaurants.Application.User.Command.AssignUserRole
{
    public class AssignUserRoleHandler(ILogger<AssignUserRoleHandler> logger,
        RoleManager<IdentityRole> roleManager,
        UserManager<Domain.Entities.User> userManager) : IRequestHandler<AssignUserRoleCommand>
    {
        public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Assign user role: {@Request}", request);

                var user = await userManager.FindByEmailAsync(request.UserEmail);
                if (user == null)
                {
                    logger.LogError("Cannot found user with {Email}", request.UserEmail);
                    throw new UnauthorizedAccessException();
                }

                var roleUser = await roleManager.FindByNameAsync(request.RoleName);
                if (roleUser == null)
                {
                    logger.LogError("Error: AssignUserRoleCommand roleUser", request.UserEmail);
                    throw new UnauthorizedAccessException();
                }
                await userManager.AddToRoleAsync(user, roleUser.Name!);
            }
            catch (Exception ex)
            {
                logger.LogError("Error: ",ex);
            }
            
        }
    }
}
