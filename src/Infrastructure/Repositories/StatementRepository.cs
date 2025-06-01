using FinancialApp.Domain.Entities;
using FinancialApp.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FinancialApp.Infrastructure.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly ApplicationDBContext _context;
        public StatementRepository(ApplicationDBContext context)
        {
             _context = context;
        }
        public async Task<Statement> create(Guid userId ,Statement statement)
        {
            var existing = await _context.Statements
               .FirstOrDefaultAsync(s => s.UserId == userId && s.StatementMonth == statement.StatementMonth);
            if (existing != null)
                throw new Exception("Statement already exists for this month.");

            await _context.Statements.AddAsync(statement);
            await _context.SaveChangesAsync();

            return statement;
        }
    }
}
