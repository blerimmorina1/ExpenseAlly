using ExpenseAlly.Application.Common.Interfaces;
using ExpenseAlly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
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

            return category.Id;
        }
    }
}
