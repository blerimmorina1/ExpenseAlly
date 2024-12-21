using ExpenseAlly.Application.Features.TransactionCategories.Commands;
using ExpenseAlly.Application.Features.TransactionCategories.Dtos;
using ExpenseAlly.Application.Features.TransactionCategories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers
{
    public class CategoriesController : ApiControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<TransactionCategoryDto>>> GetCategories()
        {
            var result = await _mediator.Send(new GetCategoriesQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var command = new CreateCategoryCommand
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type
            };
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCategories), new { id }, id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            var command = new UpdateCategoryCommand
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type
            };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = id });
            return NoContent();
        }
    }
}
