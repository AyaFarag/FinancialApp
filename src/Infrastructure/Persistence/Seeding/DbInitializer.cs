using FinancialApp.Domain.Entities;
using FinancialApp.Infrastructure.Persistence.Data;

namespace FinancialApp.Infrastructure.Persistence.Seeding
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDBContext context)
        {
            if (!context.Statements.Any())
            {
                var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

                var statement = new Statement
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    StatementMonth = new DateTime(2025, 05, 01),
                    DueDate = new DateTime(2025, 06, 10),
                    AmountDue = 1200.50m,
                    Transactions = new List<Transaction>
                {
                    new Transaction { Date = DateTime.UtcNow, Description = "Groceries", Amount = 150.25m },
                    new Transaction { Date = DateTime.UtcNow, Description = "Online Shopping", Amount = 300.00m }
                }
                };

                context.Statements.Add(statement);
                context.SaveChanges();
            }
        }
    }
}
