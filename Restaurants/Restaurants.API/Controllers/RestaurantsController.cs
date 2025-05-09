using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Constants;

namespace Restaurants.API.Controllers
{
    /// <summary>
    /// API controller for managing restaurants
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for CRUD operations on restaurants.
    /// Most endpoints require authentication, and some operations are restricted to restaurant owners.
    /// </remarks>
    [ApiController]
    [Route("api/restaurants")]
    [Authorize]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves all restaurants with optional filtering and pagination
        /// </summary>
        /// <param name="query">Query parameters for filtering and pagination</param>
        /// <returns>A list of restaurants matching the query parameters</returns>
        /// <remarks>
        /// This endpoint is accessible to anonymous users.
        /// </remarks>
        [HttpGet]
        //[Authorize (Policy = ConstantAuthentication.Created2Restaurants)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllRestaurant([FromQuery] GetAllRestaurantsQuery query)
        {
            var restaurants = await mediator.Send(query);   
            return Ok(restaurants);
        }

        /// <summary>
        /// Retrieves a specific restaurant by its ID
        /// </summary>
        /// <param name="id">The ID of the restaurant to retrieve</param>
        /// <returns>The requested restaurant if found, or NotFound if not found</returns>
        /// <remarks>
        /// This endpoint requires the user to have a nationality specified in their profile.
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Policy = ConstantAuthentication.HasNationality)]
        public async Task<IActionResult> GetRestaurant([FromRoute]int id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
            if (restaurant == null) { 
                return NotFound();
            }

            return Ok(restaurant);
        }

        /// <summary>
        /// Deletes a restaurant
        /// </summary>
        /// <param name="id">The ID of the restaurant to delete</param>
        /// <returns>NoContent if successful, NotFound if the restaurant doesn't exist, or BadRequest if the operation fails</returns>
        /// <remarks>
        /// This endpoint is restricted to restaurant owners only.
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            try
            {
                var isDeleted = await mediator.Send(new DeleteRestaurantByIdCommand(id));
                if (isDeleted)
                {
                    return NoContent();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Updates a restaurant's details
        /// </summary>
        /// <param name="request">The update request containing the new restaurant details</param>
        /// <returns>Ok if successful, NotFound if the restaurant doesn't exist, or BadRequest if the operation fails</returns>
        /// <remarks>
        /// This endpoint is restricted to restaurant owners only.
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> UpdateRestaurant([FromBody]UpdateRestaurantCommand request)
        {
            try
            {
                var isDeleted = await mediator.Send(request);
                if (isDeleted)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Creates a new restaurant
        /// </summary>
        /// <param name="request">The creation request containing the restaurant details</param>
        /// <returns>CreatedAtAction with the new restaurant's ID if successful, or BadRequest if the operation fails</returns>
        /// <remarks>
        /// This endpoint is restricted to restaurant owners only.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> CreateRestaurant([FromBody]CreateRestaurantCommand request)
        {
            try
            {
                var id = await mediator.Send(request);
                if (id != 0) 
                {
                    return CreatedAtAction(nameof(GetRestaurant),new {id}, null);
                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
