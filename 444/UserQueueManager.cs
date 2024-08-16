
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application
{


    public class UserQueueManager
    {
        private static readonly List<UserQueue> ActiveUsers = new();
        private static readonly Queue<UserQueue> WaitingQueue = new();
        private readonly ILogger<UserQueueManager> _logger;

        public UserQueueManager(ILogger<UserQueueManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool TryEnterPage(string userId, HttpContext context)
        {
            var requestPath = context.Request.Path.Value;

           
                

            if (requestPath.Contains("/AccountRegister/Logout"))
            {
                ActiveUsers.RemoveAll(u => u.UserId == userId);
                userId = "///8887776//";
            }
           
                RemoveOldUsers();

                var VerifyDuplicate = ActiveUsers.Any(u => u.UserId == userId);

                if (ActiveUsers.Count < 1 && !VerifyDuplicate&& !userId.Contains("///8887776//"))
                {
                    ActiveUsers.Add(new UserQueue { UserId = userId, StartTime = DateTime.Now });
                    _logger.LogInformation("Added new user: {UserId}. Active users count: {Count}", userId, ActiveUsers.Count);
                    return true;
                }
                else if (!userId.Contains("///8887776//"))
                {
                    WaitingQueue.Enqueue(new UserQueue { UserId = userId, StartTime = DateTime.Now });
                    _logger.LogInformation("User added to queue: {UserId}. Queue count: {Count}", userId, WaitingQueue.Count);
                    return false;
                }
                return true;

        }

        private void RemoveOldUsers()
        {
            var thresholdTime = DateTime.Now - TimeSpan.FromMinutes(1);

            // იპოვე ძველი მომხმარებლები
            var oldUsers = ActiveUsers.Where(u => u.StartTime < thresholdTime).ToList();

            // დაამატე რაოდენობა ძველი მომხმარებლების რაოდენობის მიხედვით
            int removedCount = oldUsers.Count;

            // ლოგირება ძველი მომხმარებლების `StartTime` დროებით
            if (removedCount > 0)
            {
                _logger.LogInformation("Removing {Count} old users from active list at {Time}.", removedCount, DateTime.Now);

                foreach (var user in oldUsers)
                {
                    _logger.LogInformation("User: {UserId}, StartTime: {StartTime}", user.UserId, user.StartTime);
                }

                // წაშალე ძველი მომხმარებლები სიიდან
                foreach (var user in oldUsers)
                {
                    ActiveUsers.Remove(user);

                    // დამატებითი ლოგირება: რომელი მომხმარებელი წაიშალა
                    _logger.LogInformation("Removed old user: {UserId}, StartTime: {StartTime}", user.UserId, user.StartTime);
                }

                // ლოგირება - აქტიური მომხმარებლების მიმდინარე რაოდენობა
                _logger.LogInformation("Current active users count: {CurrentCount}", ActiveUsers.Count);

            }
        }

        public void ReleaseUser()
        {

            RemoveOldUsers();

            while (ActiveUsers.Count < 1 && WaitingQueue.Count > 0)
            {
                var nextUser = WaitingQueue.Dequeue();
                ActiveUsers.Add(new UserQueue { UserId = nextUser.UserId, StartTime = DateTime.Now });
                _logger.LogInformation("User moved from queue to active: {UserId} at {Time}. Active users count: {Count}", nextUser.UserId, DateTime.Now, ActiveUsers.Count);
            }
        }



    }
}

