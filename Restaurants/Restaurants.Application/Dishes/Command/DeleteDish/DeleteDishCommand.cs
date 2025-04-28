using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Command.DeleteDish
{
    public class DeleteDishCommand (int restaurantId, int id) : IRequest<bool>
    {
        public int Id { get; set; } = id;
        public int RestaurantId { get; } = restaurantId;
    }
}
