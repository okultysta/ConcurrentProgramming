using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;

public class BallLogic : IBallManager
{
    private double width;
    private double height;
    private readonly Random random = new Random();
    private readonly IBallRepository repository;

    public BallLogic(IBallRepository repo, double initWidth, double initHeight)
    {
        repository = repo;
        width = initWidth;
        height = initHeight;
    }

    public async void CreateBall()
    {
        await Task.Run(() =>
        {
            double radius = 20;
            double x = random.NextDouble() * (width - radius);
            double y = random.NextDouble() * (height - radius);
            double speedX = random.NextDouble() * 5 - 2.5;
            double speedY = random.NextDouble() * 5 - 2.5;

            var ball = new BallThread(x, y, radius, speedX, speedY, width, height, repository);
            repository.AddBall(ball);
            ball.Start();
        });
    }

    public IEnumerable<Ball> GetCurrentBallStates()
    {
        return repository.GetAllBalls();
    }

    public async void DeleteBall()
    {
        await Task.Run(() =>
        {
            var ball = repository.GetAllBalls().FirstOrDefault();
            if (ball is BallThread bt)
            {
                bt.Stop();
                repository.removeBall(bt);
            }
        });
    }

    public void updateFrameSize(double newWidth, double newHeight)
    {
        width = newWidth;
        height = newHeight;
    }

    public void UpdateBallPositions() { } // niepotrzebne
}
