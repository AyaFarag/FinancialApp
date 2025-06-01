using FinancialApp.Application.DTOs;
using FinancialApp.Domain.Entities;

namespace FinancialApp.Infrastructure.Repositories
{
    public interface IStatementRepository
    {
        public Task<Statement> create(Guid userId,Statement statement);
    }
}
