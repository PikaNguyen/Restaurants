using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Dtos
{
    public class DishDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int? KiloCalories { get; set; }

        public static DishDTO FromEntity(Dish dishes)
        {
            return new DishDTO()
            {
                Id = dishes.Id,
                Name = dishes.Name,
                Description = dishes.Description,
                Price = dishes.Price,
                KiloCalories = dishes.KiloCalories,
            };
        }
    }
}
