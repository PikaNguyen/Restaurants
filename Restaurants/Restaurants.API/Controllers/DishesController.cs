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
    /// <summary>
    /// API controller for managing restaurant dishes
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for CRUD operations on dishes within restaurants.
    /// All endpoints require authentication, and some operations are restricted to restaurant owners.
    /// </remarks>
    [Route("api/restaurants/{restaurantId}/dishes")]
    [Authorize]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a new dish for a restaurant
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant to add the dish to</param>
        /// <param name="dishRequest">The dish creation request</param>
        /// <returns>A response indicating the result of the operation</returns>
        /// <remarks>
        /// This endpoint is restricted to restaurant owners only.
        /// The dish will be associated with the specified restaurant.
        /// </remarks>
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

        /// <summary>
        /// Retrieves all dishes for a specific restaurant
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant to get dishes for</param>
        /// <returns>A list of dishes belonging to the restaurant</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDishesForRestaurant([FromRoute]int restaurantId)
        {
            var dishes = await mediator.Send(new GetAllDishesForRestaurantQuery(restaurantId));
            return Ok(dishes);
        }

        /// <summary>
        /// Retrieves a specific dish from a restaurant
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant</param>
        /// <param name="dishId">The ID of the dish to retrieve</param>
        /// <returns>The requested dish if found, or NotFound if not found</returns>
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
