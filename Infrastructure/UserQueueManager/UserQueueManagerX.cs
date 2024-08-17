using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
namespace Infrastructure.UserQueueManager
{
    public class UserQueueManagerX
    {
        private static readonly List<UserQueue> ActiveUsers = new();
        private static readonly Queue<UserQueue> WaitingQueue = new();
        private readonly ILogger<UserQueueManagerX> _logger;

        public UserQueueManagerX(ILogger<UserQueueManagerX> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool TryEnterPage(string UserURL, HttpContext context)
        {
            var requestPath = context.Request.Path.Value;




            if (requestPath.Contains("/AccountRegister/Logout"))
            {
                ActiveUsers.RemoveAll(u => u.UserURL == UserURL);
                UserURL = "///8887776//";
                
            }

            RemoveOldUsers(context);

            var VerifyDuplicate = ActiveUsers.Any(u => u.UserURL == UserURL);

            if (ActiveUsers.Count < 1 && !VerifyDuplicate && !UserURL.Contains("///8887776//"))
            {
                ActiveUsers.Add(new UserQueue { UserURL = UserURL, StartTime = DateTime.Now }); // აქტირუი მომხარებლის დაატება List კოლეკციაში
                _logger.LogInformation("Added new user: {UserURL}. Active users count: {Count}", UserURL, ActiveUsers.Count);
                return true;
            }
            else if (!UserURL.Contains("///8887776//")) // რიგში დამატება მომხარებლის  Queue კოლექჩიში
            {
                WaitingQueue.Enqueue(new UserQueue { UserURL = UserURL, StartTime = DateTime.Now });
                _logger.LogInformation("User added to queue: {UserURL}. Queue count: {Count}", UserURL, WaitingQueue.Count);
                return false;
            }
            return true;

        }

        internal async Task RemoveOldUsers(HttpContext context)
        {
            var thresholdTime = DateTime.Now - TimeSpan.FromMinutes(1);

            var oldUsers = ActiveUsers.Where(u => u.StartTime < thresholdTime).ToList();

            int removedCount = oldUsers.Count;

            if (removedCount > 0)
            {
                _logger.LogInformation("Removing {Count} old users from active list at {Time}.", removedCount, DateTime.Now);

                foreach (var user in oldUsers)
                {
                    _logger.LogInformation("User: {UserURL}, StartTime: {StartTime}", user.UserURL, user.StartTime);
                }

                foreach (var user in oldUsers)
                {
                    ActiveUsers.Remove(user);

                    _logger.LogInformation("Removed old user: {UserURL}, StartTime: {StartTime}", user.UserURL, user.StartTime);
                }

                _logger.LogInformation("Current active users count: {CurrentCount}", ActiveUsers.Count);

            }
        }


        public void ReleaseUser(HttpContext context)
        {

            RemoveOldUsers(context);

            while (ActiveUsers.Count < 1 && WaitingQueue.Count > 0)
            {
                var nextUser = WaitingQueue.Dequeue();
                ActiveUsers.Add(new UserQueue { UserURL = nextUser.UserURL, StartTime = DateTime.Now });
                _logger.LogInformation("User moved from queue to active: {UserURL} at {Time}. Active users count: {Count}", nextUser.UserURL, DateTime.Now, ActiveUsers.Count);
            }
        }



    }
}
