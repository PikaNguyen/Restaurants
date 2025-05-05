using MediatR;

namespace Restaurants.Application.User.Command.DeleteUserRole
{
    public class DeleteUserRoleCommand : IRequest
    {
        public string UserEmail { get; set; } = default!;
        public string RoleName { get; set; } = default!;
    }
}
