using ExpenseAlly.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Commands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.TransactionCategories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category == null)
                throw new Exception("Category not found");

            category.Name = request.Name;
            category.Description = request.Description;
            category.Type = request.Type;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }


    }
}
