namespace FinancialApp.Application.DTOs
{
    public class CreditCardStatementDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime StatementMonth { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new();
    }
}
