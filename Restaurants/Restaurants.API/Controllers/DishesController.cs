using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Command;
using Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants/{restaurantId}/dishes")]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateDish([FromRoute]int restaurantId, CreateDishCommand dishRequest)
        {
            try
            {
                dishRequest.RestaurantId = restaurantId;
                await mediator.Send(dishRequest);

                return Created();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute]int restaurantId)
        {
            var dishes = mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
            return Ok(dishes);
        }

        [HttpGet("{dishId}")]
        public async Task<IActionResult> GetDishesByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dishes = mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
            return Ok(dishes);
        }
    }
}
