
namespace Dai.MiddlewareX
{
    public class PageSessionTimeoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PageSessionTimeoutMiddleware> _logger;

        public PageSessionTimeoutMiddleware(RequestDelegate next, ILogger<PageSessionTimeoutMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sessionStartTime = context.Session.GetString("SessionStartTime");
            if (sessionStartTime != null)
            {
                var startTime = DateTime.Parse(sessionStartTime);
                var elapsedTime = DateTime.Now - startTime;

                if (elapsedTime.TotalMinutes > 1) 
                {
                    _logger.LogInformation("Session expired. Redirecting to the index page.");
                    context.Response.Redirect("/Index/Home");
                    return;
                }
            }
            else
            {
                context.Session.SetString("SessionStartTime", DateTime.Now.ToString());
                         }

            await _next(context);
        }
    }
}
