using ExpenseAlly.Application.Features.TransactionCategories.Commands;
using ExpenseAlly.Application.Features.TransactionCategories.Dtos;
using ExpenseAlly.Application.Features.TransactionCategories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        // ID mismatch check
        if (id == Guid.Empty)
        {
            return BadRequest("Category ID in the URL cannot be empty.");
        }

        var command = new UpdateCategoryCommand
        {
            Id = id, // Use the ID from the URL
            Name = request.Name,
            Description = request.Description,
            Type = request.Type
        };

        try
        {
            // Send the command to Mediator
            await _mediator.Send(command);
            return NoContent(); // Successfully updated
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Category with ID {id} was not found."); // Handle case where the ID is invalid
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await _mediator.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }
}
