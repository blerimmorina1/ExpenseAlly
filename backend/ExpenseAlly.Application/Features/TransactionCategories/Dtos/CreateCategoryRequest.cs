using ExpenseAlly.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Dtos
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public TransactionType Type { get; set; }
    }
}
