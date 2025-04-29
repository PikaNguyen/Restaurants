using MediatR;

namespace Restaurants.Application.User.Command.UpdateUserDetail
{
    public class UpdateUserDetailCommand : IRequest
    {
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
