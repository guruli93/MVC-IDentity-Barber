
using Microsoft.AspNetCore.Http;

namespace Infrastructure.UserQueueManager
{
    public class QueueMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserQueueManagerX _queueManager;

        public QueueMiddleware(RequestDelegate next, UserQueueManagerX userQueueManager)
        {
            _next = next;
            _queueManager = userQueueManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.Identity.Name;



            if (!string.IsNullOrEmpty(userId))
            {
                if (!_queueManager.TryEnterPage(userId, context))
                {

                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync("Please wait, you are in queue.");
                   
                    return;
                }
            }

            try
            {
                await _next(context);
            }
            finally
            {
                _queueManager.ReleaseUser(context);
            }
        }
    }

}
