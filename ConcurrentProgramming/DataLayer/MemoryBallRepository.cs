using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class MemoryBallRepository : IBallRepository
    {
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly object _lock = new object();
        private readonly BallLogger  logger = new BallLogger("logfile.txt");

        public MemoryBallRepository(string logFilePath = null)
        {
            if (string.IsNullOrEmpty(logFilePath))
                logFilePath = "logfile.txt"; 

            logger = new BallLogger(logFilePath);
     
        }

        public void AddBall(Ball ball)
        {
            lock (_lock)
            {
                _balls.Add(ball);
            }
        }

        public IEnumerable<Ball> GetAllBalls()
        {
            lock (_lock)
            {
                return _balls.ToList(); // snapshot, bez przekazywania referencji
            }
        }

        public void removeBall(Ball ball)
        {
            lock (_lock)
            {
                _balls.Remove(ball);
            }
        }

        public void SaveBallData(Ball ball)
        {
            logger.Log(ball);
            logger.Stop();
        }
    }
}

