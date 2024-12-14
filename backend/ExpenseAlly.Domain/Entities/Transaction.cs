using ExpenseAlly.Domain.Common;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Domain.Entities
{
    public class Transaction : BaseAuditableEntity
    {
        public TransactionType Type { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }

        public TransactionCategory Category { get; set; }
    }
}
