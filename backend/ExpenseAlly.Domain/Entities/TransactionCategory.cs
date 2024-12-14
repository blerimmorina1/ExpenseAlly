using ExpenseAlly.Domain.Common;

namespace ExpenseAlly.Domain.Entities
{
    public class TransactionCategory : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
