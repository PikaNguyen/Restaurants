using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Restaurants.Application.User.Command
{
    public class UpdateUserDetailHandler(ILogger<UpdateUserDetailHandler> logger,
        IUserContext userContext,
        IUserStore<Domain.Entities.User> userStore) : IRequestHandler<UpdateUserDetailCommand>
    {
        public async Task Handle(UpdateUserDetailCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("Update user: {UserId} detail about Birth and Nationality with {@Request}", user!.Id, request);

            var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);
            if (dbUser == null) {
                logger.LogError("Cannot found user db with {UserId}", user!.Id);
                throw new UnauthorizedAccessException();
            }

            dbUser.Nationality = request.Nationality;
            dbUser.DateOfBirth = request.DateOfBirth;

            await userStore.UpdateAsync(dbUser, cancellationToken);
        }
    }
}
