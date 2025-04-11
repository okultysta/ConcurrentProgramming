using DataLayer;
using System;

namespace DataLayer
{
    public class MemoryBallRepository : IBallRepository
    {
        private readonly List<Ball> _balls = new List<Ball>();

        public void AddBall(Ball ball)
        {
            _balls.Add(ball);
        }

        public IEnumerable<Ball> GetAllBalls()
        {
            return _balls;
        }

        public void removeBall(Ball ball)
        {
            _balls.Remove(ball);
        }
    }
}
