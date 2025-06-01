namespace FinancialApp.Domain.Entities
{
    public class Statement
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } = Guid.NewGuid();
        public DateTime StatementMonth { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public byte[] RowVersion { get; set; } // For optimistic concurrency
    }
}
