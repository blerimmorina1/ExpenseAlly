﻿using ExpenseAlly.Domain.Common;
using ExpenseAlly.Domain.Enums;

namespace ExpenseAlly.Domain.Entities
{
    public class TransactionCategory : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public TransactionType Type { get; set; }

        public List<Transaction>? Transactions { get; set; }
        public List<BudgetDetail>? BudgetDetails { get; set; }
    }
}
