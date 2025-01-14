﻿using ExpenseAlly.Application.Common.Models;
using ExpenseAlly.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAlly.Application.Features.TransactionCategories.Commands
{
    public class CreateCategoryCommand : IRequest<ResponseDto>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public TransactionType Type { get; set; }
    }
}
