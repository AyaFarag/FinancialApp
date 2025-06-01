using FinancialApp.Application.Commands;
using FinancialApp.Application.DTOs;

namespace FinancialApp.Application.Services
{
    public interface IStatementService
    {
        public Task<CreditCardStatementDto> createStatement(StatementDto statement);
    }
}
