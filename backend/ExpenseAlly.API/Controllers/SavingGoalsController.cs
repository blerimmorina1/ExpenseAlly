using ExpenseAlly.Application.Features.SavingGoals.Commands;
using ExpenseAlly.Application.Features.SavingGoals.Dtos;
using ExpenseAlly.Application.Features.SavingGoals.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseAlly.API.Controllers
{
    public class SavingGoalsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public SavingGoalsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
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
            if (id != command.Id)
            {
                return BadRequest(new { Message = "Mismatched Saving Goal ID." });
            }

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSavingGoal(Guid id)
        {
            var result = await _mediator.Send(new DeleteSavingGoalCommand { Id = id });

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
    }
}