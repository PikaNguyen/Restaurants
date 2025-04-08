using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos
{
    public class RestaurantsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }
        public List<DishDTO> Dishes { get; set; } = [];

        public static RestaurantsDTO FromEntity(Restaurant restaurant)
        {
            return new RestaurantsDTO
            {
                Id = restaurant.Id,
                Category = restaurant.Category,
                Description = restaurant.Description,
                Name = restaurant.Name,
                HasDelivery = restaurant.HasDelivery,
                City = restaurant.Address?.City,
                Street = restaurant.Address?.Street,
                PostalCode = restaurant.Address?.PostalCode,
                Dishes = restaurant.Dishes.Select(DishDTO.FromEntity).ToList()
            };
        }
    }
}
