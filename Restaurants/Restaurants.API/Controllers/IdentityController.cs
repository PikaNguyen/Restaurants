using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.User.Command.AssignUserRole;
using Restaurants.Application.User.Command.DeleteUserRole;
using Restaurants.Application.User.Command.UpdateUserDetail;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers
{
    /// <summary>
    /// API controller for managing user identity and roles
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for managing user details and roles.
    /// Some operations are restricted to administrators only.
    /// </remarks>
    [ApiController]
    [Route("api/identity")]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Updates the details of the authenticated user
        /// </summary>
        /// <param name="command">The update details command containing the new user information</param>
        /// <returns>NoContent if successful, BadRequest with error message if failed</returns>
        [HttpPatch("user")]
        [Authorize]
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

        /// <summary>
        /// Assigns a role to a user
        /// </summary>
        /// <param name="command">The command containing the user ID and role to assign</param>
        /// <returns>NoContent if successful, BadRequest with error message if failed</returns>
        /// <remarks>
        /// This endpoint is restricted to administrators only.
        /// </remarks>
        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
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

        /// <summary>
        /// Removes a role from a user
        /// </summary>
        /// <param name="command">The command containing the user ID and role to remove</param>
        /// <returns>NoContent if successful, BadRequest with error message if failed</returns>
        /// <remarks>
        /// This endpoint is restricted to administrators only.
        /// </remarks>
        [HttpDelete("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteUserRole(DeleteUserRoleCommand command)
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
