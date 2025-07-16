using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyServer.Application.Commands.Category;
using MyServer.Application.Queries.Category;
using MyServer.Core.Entities;
using MyServer.Application.Models.DTOs;

namespace MyServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<CategoryEntity>> GetAllCategories(CancellationToken cancellationToken)
        {
            // Fix: Update GetAllCategoryQuery to return IEnumerable<CategoryEntity> instead of a single CategoryEntity
            return await sender.Send(new GetAllCategoriesQuery(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var category = await sender.Send(new GetCategoryByIdQuery(id), cancellationToken);
                if (category == null)
                    return NotFound();

                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the category.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryEntity category, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new AddCategoryCommand(category), cancellationToken);
            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDTO category, CancellationToken cancellationToken)
        {

            try
            {
                var updatedCategory = await sender.Send(new UpdateCategoryCommand(
                    id,
                    category.Name,
                    category.Description,
                    category.IsActive
                ), cancellationToken);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the category.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new DeleteCategoryCommand(id), cancellationToken);
                if (result)
                {
                    return NoContent(); // 204 No Content
                }
                return NotFound(); // 404 Not Found
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the category.");
            }
        }
    }

}
