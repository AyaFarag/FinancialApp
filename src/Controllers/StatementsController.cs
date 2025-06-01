using FinancialApp.Application.Commands;
using FinancialApp.Application.Queries;
using FinancialApp.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FinancialApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StatementsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StatementsController> _logger;
        private readonly IUserService _userService;
        public StatementsController(IMediator mediator, ILogger<StatementsController> logger
            , IUserService userService)
        {
            _mediator = mediator;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatements([FromQuery] string start, [FromQuery] string end, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (!DateTime.TryParse($"{start}-01", out var startDate) || !DateTime.TryParse($"{end}-01", out var endDate))
                return BadRequest("Invalid date format. Use YYYY-MM.");

            var userId = _userService.GetUserId();

            var result = await _mediator.Send(new GetStatementsInRangeQuery(startDate, endDate, userId, page, pageSize));
            return Ok(result);
        }

        [HttpGet("{month}")]
        public async Task<IActionResult> GetByMonth(string month)
        {
            if (!DateTime.TryParse($"{month}-01", out var monthDate))
                return BadRequest("Invalid date format. Use YYYY-MM.");

            var userId = _userService.GetUserId();
            var result = await _mediator.Send(new GetStatementByMonthQuery(monthDate, userId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatement([FromBody] CreateStatementCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                _logger.LogInformation("Statement created: {Id}", id);
                return Ok(id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to create statement: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }

}
