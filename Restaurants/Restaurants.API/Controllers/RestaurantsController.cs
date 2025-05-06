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
    [ApiController]
    [Route("api/restaurants")]
    [Authorize]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [Authorize (Policy = ConstantAuthentication.HasNationality)]
        public async Task<IActionResult> GetAllRestaurant()
        {
            var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        [Authorize (Policy = ConstantAuthentication.AtLeast)]
        public async Task<IActionResult> GetRestaurant([FromRoute]int id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
            if (restaurant == null) { 
                return NotFound();
            }

            return Ok(restaurant);
        }

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
