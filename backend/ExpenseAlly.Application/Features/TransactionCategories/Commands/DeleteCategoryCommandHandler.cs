using ExpenseAlly.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.TransactionCategories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category == null)
                throw new Exception("Category not found");

            _context.TransactionCategories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

      
    }
}
