using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyServer.Infrastructure.Services;
using MyServer.Application.Commands.Payment;

namespace MyServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(ISender sender) : ControllerBase
    {
        [HttpPost("checkout-session")]
        public async Task<ActionResult<CreateCheckoutSessionResponse>> CreateCheckoutSession(
            [FromBody] CreateCheckoutSessionCommand command,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await sender.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Or use proper logging like ILogger

                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
