using FluentValidation;

namespace Restaurants.Application.Dishes.Command
{
    public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
    {
        public CreateDishCommandValidator()
        {
            RuleFor(d => d.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be a non-negative number");

            RuleFor(d => d.Name)
                .Length(1, 100)
                .WithMessage("The name must be more than 1");
                
        }
    }
}
