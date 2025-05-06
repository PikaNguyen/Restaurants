using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Command;
using Restaurants.Application.Dishes.Command.DeleteDish;
using Restaurants.Application.Dishes.Queries.GetAllDishesForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants/{restaurantId}/dishes")]
    [Authorize]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
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
                return BadRequest(ex);
            }
            
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute]int restaurantId)
        {
            var dishes = await mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
            return Ok(dishes);
        }

        [HttpGet("{dishId}")]
        public async Task<IActionResult> GetDishesByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dishes = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
            if (dishes == null)
            {
                return NotFound();
            }

            return Ok(dishes);
        }

        [HttpDelete("{dishId}")]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> DeleteDishById([FromRoute]int restaurantId, [FromRoute]int dishId)
        {
            var dish = await mediator.Send(new DeleteDishCommand(restaurantId, dishId));
            if (dish)
            {

                return NoContent();
            }

            return NotFound();
        }   
    }
}
