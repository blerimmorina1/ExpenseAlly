using ExpenseAlly.Application.Features.Transactions.Dtos;
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
        public CreateTransactionRequest Transaction { get; set; }
    }
}
