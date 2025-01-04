using ExpenseAlly.Application.Common.Interfaces;

namespace ExpenseAlly.Application.Features.TransactionCategories.Commands;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
  
          var category = await _context.TransactionCategories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
        }

        category.Name = request.Name;
        category.Description = request.Description;
        category.Type = request.Type;

       
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
