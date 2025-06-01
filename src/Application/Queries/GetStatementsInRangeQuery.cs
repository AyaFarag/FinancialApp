using FinancialApp.Application.DTOs;
using MediatR;

namespace FinancialApp.Application.Queries
{
    public record GetStatementsInRangeQuery(DateTime Start, DateTime End, Guid UserId, int Page, int PageSize) : IRequest<List<CreditCardStatementDto>>;

}
