using FinancialApp.Application.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace FinancialApp.Application.Commands
{
    public class CreateStatementCommand : IRequest<StatementDto>
    {
        [JsonIgnore]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public StatementDto StatementDto { get; set; }

        public CreateStatementCommand(
            Guid userId,
            StatementDto statementDto)
        {
            UserId = userId;
            StatementDto = statementDto;
        }
    }
}
