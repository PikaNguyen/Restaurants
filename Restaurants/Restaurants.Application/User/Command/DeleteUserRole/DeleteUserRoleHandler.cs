using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Restaurants.Application.User.Command.DeleteUserRole
{
    public class DeleteUserRoleHandler(ILogger<DeleteUserRoleHandler> logger,
        UserManager<Domain.Entities.User> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IRequestHandler<DeleteUserRoleCommand>
    {
        public async Task Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Delete user role: {@Request}", request);

                var user = await userManager.FindByEmailAsync(request.UserEmail);
                if (user == null)
                {
                    logger.LogError("Cannot found user with {Email}", request.UserEmail);
                    throw new ArgumentNullException();
                }

                var roleUser = await roleManager.FindByNameAsync(request.RoleName);
                if (roleUser == null)
                {
                    logger.LogError("Error: AssignUserRoleCommand roleUser", request.UserEmail);
                    throw new ArgumentNullException();
                }

                await userManager.RemoveFromRoleAsync(user, roleUser.Name!);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error {nameof(DeleteUserRoleHandler)} : {ex} ");
            }
        }
    }
}
