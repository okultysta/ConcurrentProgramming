using System;
using System.Collections.Generic;
using DataLayer;

public class BallLogic : IBallManager
{
    private double width;
    private double height;
    private readonly Random random = new Random();
    private IBallRepository repository;

    public BallLogic(IBallRepository repo, double initWidth, double initHeight)
    {
        repository = repo;
        height = initHeight;
        width = initWidth;
    }

    public void CreateBall()
    {
        double radius = 20;
        repository.AddBall(new Ball
        {
            x = random.NextDouble() * (width - radius),
            y = random.NextDouble() * (height - radius),
            radius = radius,
            SpeedX = random.NextDouble() * 5 - 2.5,
            SpeedY = random.NextDouble() * 5 - 2.5
        });
    }

    public void UpdateBallPositions()
    {
        foreach (var ball in repository.GetAllBalls())
        {
            // Ruch kuli
            ball.Move();

            // Sprawdzenie odbicia od krawędzi
            if (ball.x < 0 || ball.x + ball.radius > width)
            {
                System.Diagnostics.Debug.WriteLine($"Odbicie w osi X przed: Ball({ball.x}, {ball.y}), SpeedX: {ball.SpeedX}");
                ball.SpeedX = -ball.SpeedX;
                System.Diagnostics.Debug.WriteLine($"Po odbiciu: Ball({ball.x}, {ball.y})");
            }

            if (ball.y < 0 || ball.y + ball.radius > height)
            {
                System.Diagnostics.Debug.WriteLine($"Odbicie w osi Y: Ball({ball.x}, {ball.y}), SpeedY: {ball.SpeedY}");
                ball.SpeedY = -ball.SpeedY;
                System.Diagnostics.Debug.WriteLine($"Po odbiciu: Ball({ball.x}, {ball.y})");
            }

            // Ustawienie pozycji kuli na planszy z uwzględnieniem promienia
            ball.x = Math.Clamp(ball.x, 0, width - ball.radius);
            ball.y = Math.Clamp(ball.y, 0, height - ball.radius);
        }

        HandleCollisions();
    }

    private void HandleCollisions()
    {
        var balls = repository.GetAllBalls().ToList();

        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                Ball b1 = balls[i];
                Ball b2 = balls[j];

                double dx = b2.x - b1.x;
                double dy = b2.y - b1.y;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                double minDist = (b1.radius + b2.radius)/2;

                if (distance < minDist && distance > 0)
                {
                    // Masa zależna od promienia (np. proporcjonalna do pola)
                    double m1 = Math.PI * b1.radius * b1.radius;
                    double m2 = Math.PI * b2.radius * b2.radius;

                    // Normalny wektor zderzenia
                    double nx = dx / distance;
                    double ny = dy / distance;

                    // Tangencjalny wektor (prostopadły)
                    double tx = -ny;
                    double ty = nx;

                    // Projekcja prędkości na osie normalną i tangencjalną
                    double v1n = b1.SpeedX * nx + b1.SpeedY * ny;
                    double v1t = b1.SpeedX * tx + b1.SpeedY * ty;
                    double v2n = b2.SpeedX * nx + b2.SpeedY * ny;
                    double v2t = b2.SpeedX * tx + b2.SpeedY * ty;

                    // Zderzenie sprężyste: wymiana pędu na osi normalnej
                    double v1nAfter = (v1n * (m1 - m2) + 2 * m2 * v2n) / (m1 + m2);
                    double v2nAfter = (v2n * (m2 - m1) + 2 * m1 * v1n) / (m1 + m2);

                    // Ostateczne prędkości
                    b1.SpeedX = v1nAfter * nx + v1t * tx;
                    b1.SpeedY = v1nAfter * ny + v1t * ty;
                    b2.SpeedX = v2nAfter * nx + v2t * tx;
                    b2.SpeedY = v2nAfter * ny + v2t * ty;

                    // Delikatna korekta pozycji – uniknięcie nakładania
                    double overlap = 0.5 * (minDist - distance);
                    b1.x -= overlap * nx;
                    b1.y -= overlap * ny;
                    b2.x += overlap * nx;
                    b2.y += overlap * ny;
                }
            }
        }
    }



    public IEnumerable<Ball> GetCurrentBallStates() => repository.GetAllBalls();

    public void DeleteBall()
    {
        repository.removeBall(repository.GetAllBalls().First());
    }

    public void updateFrameSize(double newWidth, double newHeight)
    {
        width = newWidth;
        height = newHeight;
    }
}
