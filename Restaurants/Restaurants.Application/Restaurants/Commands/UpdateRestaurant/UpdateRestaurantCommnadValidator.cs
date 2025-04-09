using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommnadValidator : AbstractValidator<UpdateRestaurantCommand>
    {
        public UpdateRestaurantCommnadValidator()
        {
            RuleFor(dto => dto.Name)
                .Length(3, 100)
                .WithMessage("Invalid length while updating name.")
                .NotEmpty();

            RuleFor(dto => dto.Description)
                .Length(0, 255)
                .WithMessage("Invalid length while updating Description.");
        }
    }
}
