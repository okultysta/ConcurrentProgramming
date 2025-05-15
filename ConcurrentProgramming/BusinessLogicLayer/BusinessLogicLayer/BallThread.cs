using System;
using System.Threading;

namespace DataLayer
{
    public class BallThread : Ball
    {
        private readonly double width;
        private readonly double height;
        private readonly int delay;
        private readonly IBallRepository repository;
        private Thread thread;
        private bool running = true;
        private readonly object _lock = new object();

        public BallThread(double x, double y, double radius, double speedX, double speedY, double width, double height, IBallRepository repository, int delay = 16)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.SpeedX = speedX;
            this.SpeedY = speedY;
            this.width = width;
            this.height = height;
            this.repository = repository;
            this.delay = delay;
        }

        public void Start()
        {
            thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        private void Run()
        {
            while (running)
            {
                lock (_lock)
                {
                    Move();
                    HandleWallCollision();
                    HandleCollisions();
                }

                Thread.Sleep(delay);
            }
        }

        private void HandleWallCollision()
        {
            if (x < 0 || x + radius > width)
                SpeedX = -SpeedX;

            if (y < 0 || y + radius > height)
                SpeedY = -SpeedY;

            x = Math.Clamp(x, 0, width - radius);
            y = Math.Clamp(y, 0, height - radius);
        }

        private void HandleCollisions()
        {
            foreach (var other in repository.GetAllBalls())
            {
                if (other == this) continue;

                double dx = other.x - this.x;
                double dy = other.y - this.y;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                double minDist = (this.radius + other.radius)/2;

                if (distance < minDist && distance > 0.0)
                {
                    // Wektor normalny (od this do other)
                    double nx = dx / distance;
                    double ny = dy / distance;

                    // Wektor styczny
                    double tx = -ny;
                    double ty = nx;

                    // Prędkości w kierunku normalnym i stycznym
                    double v1n = this.SpeedX * nx + this.SpeedY * ny;
                    double v1t = this.SpeedX * tx + this.SpeedY * ty;
                    double v2n = other.SpeedX * nx + other.SpeedY * ny;
                    double v2t = other.SpeedX * tx + other.SpeedY * ty;

                    // Sprężyste zderzenie – wymiana składowych normalnych
                    double v1nAfter = v2n;
                    double v2nAfter = v1n;

                    // Nowe wektory prędkości
                    this.SpeedX = v1nAfter * nx + v1t * tx;
                    this.SpeedY = v1nAfter * ny + v1t * ty;
                    other.SpeedX = v2nAfter * nx + v2t * tx;
                    other.SpeedY = v2nAfter * ny + v2t * ty;

                    // Korekcja pozycji – zapobiega „wpadaniu w siebie”
                    double overlap = 0.5 * (minDist - distance + 0.1);
                    this.x -= overlap * nx;
                    this.y -= overlap * ny;
                    other.x += overlap * nx;
                    other.y += overlap * ny;
                }
            }
        }
    }
}
