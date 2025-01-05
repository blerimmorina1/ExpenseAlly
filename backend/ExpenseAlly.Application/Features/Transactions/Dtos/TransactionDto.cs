using ExpenseAlly.Application.Features.TransactionCategories.Dtos;
using ExpenseAlly.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.Transactions.Dtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public TransactionCategoryDto Category { get; set; }
        public TransactionType CategoryType { get; set; }
    }
}
