using MediatR;

namespace Restaurants.Application.User.Command
{
    public class UpdateUserDetailCommand : IRequest
    {
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
