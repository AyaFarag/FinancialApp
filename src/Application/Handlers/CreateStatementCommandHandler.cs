using AutoMapper;
using FinancialApp.Application.Commands;
using FinancialApp.Application.DTOs;
using FinancialApp.Application.Services;
using FinancialApp.Domain.Entities;
using FinancialApp.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialApp.Application.Handlers
{
    public class CreateStatementCommandHandler : IRequestHandler<CreateStatementCommand, StatementDto>
    {
        private readonly IStatementService _statementService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateStatementCommandHandler> _logger;

        public CreateStatementCommandHandler(
            IStatementService statementService, 
            IMapper mapper,
            ILogger<CreateStatementCommandHandler> logger)
        {
            _statementService = statementService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StatementDto> Handle(CreateStatementCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting creation of statement for UserId: {UserId}, Month: {Month}", request.UserId, request.StatementDto.StatementMonth);

            const int maxRetryCount = 3;
            int retryAttempts = 0;

            while (retryAttempts < maxRetryCount)
            {
                try
                {
                    var statement = await _statementService.createStatement(request.StatementDto);
                    _logger.LogInformation("Statement created successfully with Id: {StatementId} for UserId: {UserId}", statement.Id, request.UserId);
                    return _mapper.Map<StatementDto>(statement);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retryAttempts++;
                    _logger.LogWarning(ex, "Concurrency conflict detected. Retry {Attempt}/{Max}", retryAttempts, maxRetryCount);

                    if (retryAttempts >= maxRetryCount)
                    {
                        _logger.LogError(ex, "Max retries reached. Aborting.");
                        throw;
                    }

                    // Reload the entity and retry
                    foreach (var entry in ex.Entries)
                    {
                        await entry.ReloadAsync(cancellationToken);
                    }
                }
            }
            throw new InvalidOperationException("Failed to save statement after retries.");
        }
    }
}
