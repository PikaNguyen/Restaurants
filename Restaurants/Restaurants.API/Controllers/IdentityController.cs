using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.User.Command;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPatch("user")]
        public async Task<IActionResult> UpdateUserDetail(UpdateUserDetailCommand command)
        {
            try
            {
                await mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }
    }
}
