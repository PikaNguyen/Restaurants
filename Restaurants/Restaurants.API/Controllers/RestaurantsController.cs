using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Domain.Repositories;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRestaurant()
        {
            var restaurants = await restaurantsService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant([FromRoute]int id)
        {
            var restaurant = await restaurantsService.GetRestaurantAsync(id);
            if (restaurant == null) { 
                return NotFound();
            }
            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody]RestaurantRequest request)
        {
            try
            {
                var id = await restaurantsService.CreateNewRestaurant(request);
                if (id != 0) {

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
