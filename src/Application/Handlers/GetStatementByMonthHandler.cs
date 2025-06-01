using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinancialApp.Application.DTOs;
using FinancialApp.Application.Queries;
using FinancialApp.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FinancialApp.Application.Handlers
{
    public class GetStatementByMonthHandler : IRequestHandler<GetStatementByMonthQuery, CreditCardStatementDto?>
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GetStatementByMonthHandler> _logger;

        public GetStatementByMonthHandler(ApplicationDBContext context, 
            IMapper mapper, 
            IMemoryCache cache,
            ILogger<GetStatementByMonthHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<CreditCardStatementDto?> Handle(GetStatementByMonthQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching statement for UserId: {UserId}, Month: {Month}", request.UserId, request.Month);
            var retryAttempts = 0;
            const int maxRetryCount = 3;

            while (retryAttempts < maxRetryCount)
            {
                try
                {

                    string cacheKey = $"Statement_{request.UserId}_{request.Month:yyyyMM}";

                    if (!_cache.TryGetValue(cacheKey, out CreditCardStatementDto? statement))
                    {
                        var entity = await _context.Statements
                            .Where(s => s.UserId == request.UserId && s.StatementMonth.Year == request.Month.Year && s.StatementMonth.Month == request.Month.Month)
                            .ProjectTo<CreditCardStatementDto>(_mapper.ConfigurationProvider)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(cancellationToken);

                        if (entity != null)
                        {
                            _cache.Set(cacheKey, entity, TimeSpan.FromSeconds(30));
                            statement = entity;
                            _logger.LogInformation("Retrieved statement Id: {StatementId}", statement.Id);

                        }

                        if (statement == null)
                        {
                            _logger.LogWarning("No statement found for UserId: {UserId}, Month: {Month}", request.UserId, request.Month);
                            return null;
                        }
                    }
                    return statement;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retryAttempts++;
                    _logger.LogWarning(ex, "Concurrency conflict on read. Retry {Attempt}/{Max}", retryAttempts, maxRetryCount);
                    if (retryAttempts >= maxRetryCount)
                        throw;
                }
            }

            throw new InvalidOperationException("Failed to read statement after retries.");
        }
    }
}
