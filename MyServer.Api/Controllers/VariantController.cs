using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyServer.Core.Entities.ProductEntities;
using MyServer.Application.Commands.Variant;
using MyServer.Application.Models.DTOs.CommandsDTO.Variant;
//using MyServer.Application.Queries.Variant;


namespace MyServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantController(ISender sender): ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> AddVariant([FromBody] CreateVariantDTO variant)
        {
            var result = await sender.Send(new CreateVariantCommand(variant));

            return Ok(result);
        }


    }
}
