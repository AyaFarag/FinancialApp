namespace FinancialApp.Application.DTOs
{
    public class StatementDto
    {
        public DateTime StatementMonth { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}
