using FinancialApp.Application.DTOs;
using MediatR;

namespace FinancialApp.Application.Queries
{
    public record GetStatementByMonthQuery(DateTime Month, Guid UserId) : IRequest<CreditCardStatementDto?>;
}
