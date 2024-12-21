namespace ExpenseAlly.Application.Common.Models
{
    public class ResponseDto
    {
        public bool Success { get; set; }
        public IEnumerable<ErrorDto>? Errors { get; set; }
    }
}
