using System.Diagnostics;
namespace Dai.MiddlewareX
{
   

    public class PerformanceMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMonitoringMiddleware> _logger;

        public PerformanceMonitoringMiddleware(RequestDelegate next, ILogger<PerformanceMonitoringMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation($"Request to-------------> {context.Request.Path} took {elapsed} ms.");
        }
    }

}
