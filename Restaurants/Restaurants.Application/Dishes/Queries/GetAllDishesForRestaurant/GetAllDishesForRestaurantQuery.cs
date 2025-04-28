using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant
{
    public class GetAllDishesForRestaurantQuery(int restaurantId) : IRequest<IEnumerable<DishDTO>>
    {
        public int RestaurantId { get; } = restaurantId;
    }
}
