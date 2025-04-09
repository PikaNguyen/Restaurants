using FluentValidation;

namespace Restaurants.Application.Restaurants.Validators;

public class RestaurantRequestValidator : AbstractValidator<RestaurantRequest>
{
    private readonly List<string> listCategories = new List<string>() { "Danchoi", "VietNam", "My"};
    //Should validator with FluentValidator instead of validating model in DTO 
    public RestaurantRequestValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Category)
            .Must(cate => listCategories.Any(c=> string.Equals(c, cate, StringComparison.OrdinalIgnoreCase)))
            //.Must(listCategories.Contains)
            //.Must(category => listCategories.Contains(category))
            .WithMessage("Invalid category. Please choose from the valid category ");
            /*.Custom((value, context) =>
            {
                var isValidCategory = listCategories.Contains(value);
                if (!isValidCategory)
                {
                    context.AddFailure("Category", "Invalid category. Please choose from the valid category");
                }
            });*/

        RuleFor(dto => dto.Description)
            .Length(0,255)
            .NotEmpty();

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email's information");

    }
}
