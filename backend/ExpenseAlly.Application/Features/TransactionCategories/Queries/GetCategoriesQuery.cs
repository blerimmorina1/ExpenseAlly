using ExpenseAlly.Application.Features.TransactionCategories.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Queries
{
    public class GetCategoriesQuery : IRequest<List<TransactionCategoryDto>>
    {
    }
}
