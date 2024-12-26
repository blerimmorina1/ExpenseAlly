using ExpenseAlly.Application.Features.Transactions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.Transactions.Queries
{
    public class GetTransactionsQuery : IRequest<List<TransactionDto>>
    {
      
    }
}
