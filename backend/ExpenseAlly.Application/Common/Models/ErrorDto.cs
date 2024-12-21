
namespace ExpenseAlly.Application.Common.Models;

public class ErrorDto
{
    public string Code { get; set; } 
    public string Message { get; set; } 
    public string? Details { get; set; }
}
