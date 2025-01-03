using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.SavingGoals.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.SavingGoals.Queries
{
    public class GetSavingGoalQueryHandler : IRequestHandler<GetSavingGoalQuery, ResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetSavingGoalQueryHandler> _logger;

        public GetSavingGoalQueryHandler(IApplicationDbContext context, ILogger<GetSavingGoalQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseDto> Handle(GetSavingGoalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var savingGoal = await _context.SavingGoals
                    .Where(s => s.Id == request.Id)
                    .Select(s => new SavingGoalDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        TargetAmount = s.TargetAmount,
                        CurrentAmount = s.CurrentAmount,
                        Deadline = s.Deadline,
                        IsCompleted = s.IsCompleted,
                        Notes = s.Notes
                    })
                    .FirstOrDefaultAsync(cancellationToken);
                
                if (savingGoal == null)
                {
                    return new ResponseDto
                    {
                        Errors = new List<ErrorDto>
                        {
                            new ErrorDto
                            {
                                Code = "NotFound",
                                Message = "Saving goal not found."
                            }
                        }
                    };
                }
                
                return new ResponseDto
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the saving goal.");
                return new ResponseDto
                {
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto
                        {
                            Code = "InternalServerError",
                            Message = "An error occurred while fetching the saving goal."
                        }
                    }
                };
            }
        }
    }
}