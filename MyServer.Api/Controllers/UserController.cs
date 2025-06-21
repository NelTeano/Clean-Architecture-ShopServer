using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyServer.Core.Entities; 
using MyServer.Application.Commands;

namespace MyServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ISender sender) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody] UserEntity user)
        {
            var result = await sender.Send(new AddUserCommand(user));

            return Ok(user); // Return the added user or a success message
        }

    }
}
