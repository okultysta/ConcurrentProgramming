using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using DataLayer;
using System.Collections.Generic;

[TestClass]
public class BallLogicTests
{
    private const double Width = 800;
    private const double Height = 600;

    [TestMethod]
    public void CreateBall_Should_AddBall_To_Repository()
    {
        var repo = new TestBallRepository();
        var logic = new BallLogic(repo, Width, Height);

        logic.CreateBall();

        // Poczekaj chwilę, bo dodawanie jest asynchroniczne
        Thread.Sleep(100);

        Assert.AreEqual(1, repo.Balls.Count);

        var ball = repo.Balls.First();

        Assert.IsTrue(ball.x >= ball.radius && ball.x <= Width - ball.radius, $"x={ball.x}");
        Assert.IsTrue(ball.y >= ball.radius && ball.y <= Height - ball.radius, $"y={ball.y}");
        Assert.IsTrue(ball.SpeedX >= -2.5 && ball.SpeedX <= 2.5, $"SpeedX={ball.SpeedX}");
        Assert.IsTrue(ball.SpeedY >= -2.5 && ball.SpeedY <= 2.5, $"SpeedY={ball.SpeedY}");
        Assert.AreEqual(20, ball.radius);
    }

    [TestMethod]
    public void DeleteBall_Should_Remove_FirstBall_And_StopThread()
    {
        var repo = new TestBallRepository();
        var logic = new BallLogic(repo, Width, Height);

        logic.CreateBall();
        Thread.Sleep(100); // daj czas na stworzenie i start wątku

        Assert.AreEqual(1, repo.Balls.Count);

        var ballThread = repo.Balls.First() as BallThread;
        Assert.IsNotNull(ballThread);
        Assert.IsTrue(ballThread != null);

        logic.DeleteBall();
        Thread.Sleep(100); // daj czas na zatrzymanie i usunięcie

        Assert.AreEqual(0, repo.Balls.Count);
        Assert.IsFalse(ballThread.IsRunning);
    }

    [TestMethod]
    public void updateFrameSize_Should_Update_FrameSizeProvider()
    {
        var repo = new TestBallRepository();
        var logic = new BallLogic(repo, 100, 100);

        logic.updateFrameSize(1024, 768);

        logic.CreateBall();
        Thread.Sleep(100);

        var ball = repo.Balls.First();
        Assert.IsTrue(ball.x >= ball.radius && ball.x <= 1024 - ball.radius);
        Assert.IsTrue(ball.y >= ball.radius && ball.y <= 768 - ball.radius);
    }

    [TestMethod]
    public void BallThread_Should_Bounce_Off_Walls()
    {
        var repo = new TestBallRepository();

        // Kula na krawędzi, lecąca w stronę ściany
        var ball = new BallThread(Width - 19, Height - 19, 20, 10, 10, new FrameSizeProvider(Width, Height), repo, delay: 10);
        repo.AddBall(ball);

        ball.Start();

        // Czekamy na kilka aktualizacji pozycji
        Thread.Sleep(200);

        ball.Stop();

        Assert.IsTrue(ball.SpeedX < 0, "SpeedX powinno się odbić i być ujemne");
        Assert.IsTrue(ball.SpeedY < 0, "SpeedY powinno się odbić i być ujemne");
        Assert.IsTrue(ball.x <= Width - ball.radius);
        Assert.IsTrue(ball.y <= Height - ball.radius);
    }

    [TestMethod]
    public void BallThread_Should_Handle_Collision_With_Another_Ball()
    {
        var repo = new TestBallRepository();

        var ball1 = new BallThread(100, 100, 20, 1, 0, new FrameSizeProvider(Width, Height), repo, delay: 10);
        var ball2 = new BallThread(115, 100, 20, -1, 0, new FrameSizeProvider(Width, Height), repo, delay: 10);

        repo.AddBall(ball1);
        repo.AddBall(ball2);

        ball1.Start();
        ball2.Start();

        Thread.Sleep(300);

        ball1.Stop();
        ball2.Stop();

        // Po kolizji prędkości powinny się zmienić
        Assert.IsTrue(ball1.SpeedX < 1, "ball1.SpeedX powinno zmaleć");
        Assert.IsTrue(ball2.SpeedX > -1, "ball2.SpeedX powinno wzrosnąć");
    }

    #region Test Double for IBallRepository

    private class TestBallRepository : IBallRepository
    {
        public List<Ball> Balls { get; } = new List<Ball>();

        public void AddBall(Ball b)
        {
            Balls.Add(b);
        }

        public IEnumerable<Ball> GetAllBalls()
        {
            return Balls;
        }

        public void removeBall(Ball b)
        {
            Balls.Remove(b);
        }

        public void SaveBallData(Ball ball)
        {
            // Tu nic nie robimy, to mock
        }
    }

    #endregion
}
