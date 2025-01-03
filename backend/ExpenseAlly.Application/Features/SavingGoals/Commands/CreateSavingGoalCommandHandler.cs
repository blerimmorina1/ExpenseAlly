using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ExpenseAlly.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace ExpenseAlly.Application.Features.SavingGoals.Commands
{
    public class CreateSavingGoalCommandHandler : IRequestHandler<CreateSavingGoalCommand, ResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IValidator<CreateSavingGoalCommand> _validator;
        private readonly ILogger<CreateSavingGoalCommandHandler> _logger;

        public CreateSavingGoalCommandHandler(IApplicationDbContext context, IValidator<CreateSavingGoalCommand> validator, ILogger<CreateSavingGoalCommandHandler> logger)
        {
            _context = context;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ResponseDto> Handle(CreateSavingGoalCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ResponseDto
                {
                    Errors = validationResult.Errors.Select(e => new ErrorDto
                    {
                        Code = "ValidationError",
                        Message = e.ErrorMessage
                    })
                };
            }

            try
            {
                // Create a new SavingGoal entity
                var savingGoal = new SavingGoal
                {
                    Name = request.Name,
                    TargetAmount = request.TargetAmount,
                    CurrentAmount = request.CurrentAmount,
                    Deadline = request.Deadline,
                    IsCompleted = request.IsCompleted,
                    Notes = request.Notes
                };

                // Add SavingGoal to the database
                _context.SavingGoals.Add(savingGoal);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseDto
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the saving goal.");
                return new ResponseDto
                {
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto
                        {
                            Code = "InternalServerError",
                            Message = "An error occurred while creating the saving goal."
                        }
                    }
                };
            }
        }
    }
}