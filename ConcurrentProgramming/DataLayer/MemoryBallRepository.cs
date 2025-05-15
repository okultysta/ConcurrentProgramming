using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class MemoryBallRepository : IBallRepository
    {
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly object _lock = new object();

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
    }
}

