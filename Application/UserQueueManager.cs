
namespace Application
{
    public class UserQueueManager
    {
        private static readonly List<UserQueue> ActiveUsers = new();
        private static readonly Queue<UserQueue> WaitingQueue = new();

        public bool TryEnterPage(string userId)
        {
            ActiveUsers.RemoveAll(u => DateTime.Now - u.StartTime > TimeSpan.FromMinutes(5));

            if (ActiveUsers.Count < 10)
            {
                ActiveUsers.Add(new UserQueue { UserId = userId, StartTime = DateTime.Now });
                return true; 
            }
            else
            {
                WaitingQueue.Enqueue(new UserQueue { UserId = userId, StartTime = DateTime.Now });
                return false; 
            }
        }

        public void ReleaseUser(string userId)
        {
            ActiveUsers.RemoveAll(u => u.UserId == userId);

            if (WaitingQueue.Count > 0)
            {
                var nextUser = WaitingQueue.Dequeue();
                ActiveUsers.Add(new UserQueue { UserId = nextUser.UserId, StartTime = DateTime.Now });
            }
        }
    }

}
