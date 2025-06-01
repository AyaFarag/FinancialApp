using FinancialApp.Application.Commands;
using FluentValidation;

namespace FinancialApp.Application.Validators
{
    public class CreateStatementCommandValidator : AbstractValidator<CreateStatementCommand>
    {
        public CreateStatementCommandValidator()
        {
            RuleFor(x => x.StatementDto.AmountDue).GreaterThanOrEqualTo(0);
            RuleFor(x => x.StatementDto.StatementMonth).LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.StatementDto.DueDate).LessThanOrEqualTo(DateTime.UtcNow);
            RuleForEach(x => x.StatementDto.Transactions).ChildRules(trans =>
            {
                trans.RuleFor(t => t.AmountDue).GreaterThanOrEqualTo(0);
                trans.RuleFor(t => t.DueDate).LessThanOrEqualTo(DateTime.UtcNow);
            });
        }
    }
}
