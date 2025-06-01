namespace FinancialApp.API.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log Request
            _logger.LogInformation("Incoming Request: {method} {url}", context.Request.Method, context.Request.Path);

            // Copy original response body
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context); 

                // Read response body
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation("Response {statusCode}: {responseBody}", context.Response.StatusCode, text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing the request.");
                throw; // Re-throw the exception to let pipeline handle it
            }
            finally
            {
                // Copy back the original body stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

}
