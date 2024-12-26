using ExpenseAlly.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<Guid>
    {
        public TransactionType Type { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
