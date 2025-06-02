using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class MemoryBallRepository : IBallRepository, IDisposable
    {
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly object _lock = new object();
        private readonly BallLogger _logger;
        private bool disposed = false;

        public MemoryBallRepository(string logFilePath = null)
        {
            if (string.IsNullOrEmpty(logFilePath))
                logFilePath = "logfile.txt";

            _logger = new BallLogger(logFilePath);
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
                return _balls.ToList(); // snapshot
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
            _logger.Log(ball);
        }

        public void Dispose()
        {
            if (disposed) return;
            _logger.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
