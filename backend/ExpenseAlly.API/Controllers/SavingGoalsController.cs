using ExpenseAlly.Application.Features.SavingGoals.Commands;
using ExpenseAlly.Application.Features.SavingGoals.Dtos;
using ExpenseAlly.Application.Features.SavingGoals.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers;

public class SavingGoalsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateSavingGoal([FromBody] CreateSavingGoalCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateSavingGoal), new { id = result }, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<SavingGoalDto>>> GetSavingGoals([FromQuery] GetSavingGoalQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSavingGoal(Guid id, [FromBody] UpdateSavingGoalCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSavingGoal(Guid id)
    {
        var result = await _mediator.Send(new DeleteSavingGoalCommand { Id = id });

        return Ok(result);
    }

    [HttpPost("{id}/Contribute")]
    public async Task<IActionResult> Contribute(Guid id, [FromBody] ContributeSavingGoalCommand command)
    {
        if (id != command.SavingGoalId)
        {
            return BadRequest(new { Message = "Mismatched Saving Goal ID." });
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
