using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant
{
    public class GetDishByIdForRestaurantQuery(int restaurantId, int dishId) : IRequest<DishDTO>
    {
        public int DishId { get; } = dishId;
        public int RestaurantId { get; } = restaurantId;
    }
}
