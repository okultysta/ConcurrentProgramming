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
