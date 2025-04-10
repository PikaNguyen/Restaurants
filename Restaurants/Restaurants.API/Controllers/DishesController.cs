using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Command;

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
        public async Task<IActionResult> GetAllDishes()
        {

            return Ok();
        }
    }
}
