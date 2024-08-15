using Application;

namespace Dai.MiddlewareX
{
    public class QueueMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly UserQueueManager _queueManager = new();

        public QueueMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.Identity.Name;
           var userId2 = context.User.Identity.AuthenticationType;

            if (!_queueManager.TryEnterPage(userId))
            {
           
                context.Response.StatusCode = 429; 
                await context.Response.WriteAsync("Please wait, you are in queue.");
                return;
            }

            try
            {
                await _next(context); 
            }
            finally
            {
                _queueManager.ReleaseUser(userId); 
            }
        }
    }

}
