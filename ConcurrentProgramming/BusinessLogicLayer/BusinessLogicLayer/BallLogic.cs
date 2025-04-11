using System;
using System.Collections.Generic; // Upewnij się, że dodano tę przestrzeń nazw
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
        // Tworzymy kulę z losową prędkością
        repository.AddBall(new Ball
        {
            x = random.NextDouble() * width,
            y = random.NextDouble() * height,
            radius = 20,
            SpeedX = random.NextDouble() * 5 - 2.5, // Losowa prędkość w osi X (-2.5 do 2.5)
            SpeedY = random.NextDouble() * 5 - 2.5  // Losowa prędkość w osi Y (-2.5 do 2.5)
        });
    }



    public void UpdateBallPositions()
    {
        foreach (var ball in repository.GetAllBalls())
        {
            // Ruch kuli
            ball.Move();

            // Odbicie od krawędzi (bocznej i górnej/dolnej)
            if (ball.x - ball.radius < 0 || ball.x + ball.radius > width)
            {
                ball.SpeedX = -ball.SpeedX; // Zmiana kierunku w osi X
            }

            if (ball.y - ball.radius < 0 || ball.y + ball.radius > height)
            {
                ball.SpeedY = -ball.SpeedY; // Zmiana kierunku w osi Y
            }

            // Upewnij się, że kula nie wychodzi poza ekran
            ball.x = Math.Clamp(ball.x, ball.radius, width - ball.radius);
            ball.y = Math.Clamp(ball.y, ball.radius, height - ball.radius);

            // Debugowanie
            //System.Diagnostics.Debug.WriteLine($"Ball moved to ({ball.x}, {ball.y}) with speed ({ball.SpeedX}, {ball.SpeedY})");
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
