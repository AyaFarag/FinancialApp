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
    public class GetStatementsInRangeHandler : IRequestHandler<GetStatementsInRangeQuery, List<CreditCardStatementDto>>
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GetStatementsInRangeHandler> _logger;

        public GetStatementsInRangeHandler(ApplicationDBContext context,
            IMapper mapper,
            IMemoryCache cache,
            ILogger<GetStatementsInRangeHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<CreditCardStatementDto>> Handle(GetStatementsInRangeQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching statements for UserId: {UserId}, Start: {Start}, End: {End}", request.UserId, request.Start, request.End);

            var retryAttempts = 0;
            const int maxRetryCount = 3;

            while (retryAttempts < maxRetryCount)
            {
                try
                {
                    string cacheKey = $"Statements_{request.UserId}_{request.Start:yyyyMM}_{request.End:yyyyMM}_{request.Page}_{request.PageSize}";

                    if (!_cache.TryGetValue(cacheKey, out List<CreditCardStatementDto> statements))
                    {
                        statements = await _context.Statements
                            .Where(s => s.UserId == request.UserId && s.StatementMonth >= request.Start && s.StatementMonth <= request.End)
                            .OrderByDescending(s => s.StatementMonth)
                            .Skip((request.Page - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ProjectTo<CreditCardStatementDto>(_mapper.ConfigurationProvider)
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);

                        _cache.Set(cacheKey, statements, TimeSpan.FromSeconds(30));

                        _logger.LogInformation("Retrieved {Count} statements for UserId: {UserId}", statements.Count, request.UserId);

                    }
                    return statements;
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
