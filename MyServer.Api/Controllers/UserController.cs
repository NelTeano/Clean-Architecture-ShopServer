using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyServer.Application.Commands.User;
using MyServer.Application.Commands.User;
using MyServer.Application.Models.DTOs;
using MyServer.Application.Queries.User;
using MyServer.Application.Queries.User;
using MyServer.Core.Entities; 

namespace MyServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(ISender sender) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody] UserEntity user)
        {
            var result = await sender.Send(new AddUserCommand(user));

            return Ok(user); // Return the added user or a success message
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await sender.Send(new GetUserByIdQuery(new UserEntity { Id = id }));
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await sender.Send(new GetAllUserQuery(), cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving all users: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDTO user)
        {
            try
            {
                var updatedUser = await sender.Send(new UpdateUserCommand(
                    id,
                    user.Username,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    user.EmailConfirmed,
                    user.IsActive,
                    user.CreatedOn,
                    user.LastLogin,
                    user.Role
                ));
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await sender.Send(new DeleteUserCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the user: " + ex.Message);
            }
        }
    } 
}
