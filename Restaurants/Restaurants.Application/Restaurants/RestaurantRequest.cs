using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Restaurants
{
    public class RestaurantRequest
    {
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = default!;
        public string? City { get; set; }
        public string? Street { get; set; }
        [Phone]
        public string? PostalCode { get; set; }
        public string Description { get; set; } = default!;
        [Required(ErrorMessage ="Invalid Category ")]
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }

        [EmailAddress]
        public string ContactEmail { get; set; } = default!;

        public string ContactName { get; set; } = default!;


    }
}
