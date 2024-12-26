using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Application.Features.Transactions.Commands;
using ExpenseAlly.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IValidator<CreateCategoryCommand> _validator;
        private readonly ILogger<CreateTransactionCommandHandler> _logger;

        public CreateCategoryCommandHandler(IApplicationDbContext context, IValidator<CreateCategoryCommand> validator, ILogger<CreateTransactionCommandHandler> logger)
        {
            _context = context;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ResponseDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

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

                var category = new TransactionCategory
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Type = request.Type
                };

                _context.TransactionCategories.Add(category);
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseDto
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the transaction category.");
                return new ResponseDto
                {
                    Errors = new List<ErrorDto>
                    {
                        new ErrorDto
                        {
                            Code = "InternalServerError",
                            Message = "An error occurred while creating the transaction category."
                        }
                    }
                };
            }
        }
    }
}
