using System.Text.Json.Serialization;

namespace FinancialApp.Application.DTOs
{
    public class TransactionDto
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime StatementMonth { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public string? Description { get; set; }
    }
}
