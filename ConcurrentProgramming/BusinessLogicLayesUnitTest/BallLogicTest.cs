using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using DataLayer;

[TestClass]
public class BallLogicTests
{
    [TestMethod]
    public void CreateBall_Should_AddBall_To_Repository()
    {
        // Arrange
        var repo = new TestBallRepository();
        var logic = new BallLogic(repo, 800, 600);

        // Act
        logic.CreateBall();

        // Assert
        Assert.AreEqual(1, repo.Balls.Count);
        var ball = repo.Balls.First();
        Assert.IsTrue(ball.x >= 0 && ball.x <= 800);
        Assert.IsTrue(ball.y >= 0 && ball.y <= 600);
        Assert.IsTrue(ball.SpeedX >= -2.5 && ball.SpeedX <= 2.5);
        Assert.IsTrue(ball.SpeedY >= -2.5 && ball.SpeedY <= 2.5);
        Assert.AreEqual(20, ball.radius);
    }

    [TestMethod]
    public void UpdateBallPositions_Should_Clamp_And_Bounce()
    {
        // Arrange
        var repo = new TestBallRepository();
        var ball = new Ball { x = 790, y = 590, radius = 20, SpeedX = 15, SpeedY = 15 };
        repo.Balls.Add(ball);
        var logic = new BallLogic(repo, 800, 600);

        // Act
        logic.UpdateBallPositions();

        // Assert
        Assert.IsTrue(ball.SpeedX < 0); // powinien się odbić w poziomie
        Assert.IsTrue(ball.SpeedY < 0); // powinien się odbić w pionie
        Assert.IsTrue(ball.x <= 800 - ball.radius);
        Assert.IsTrue(ball.y <= 600 - ball.radius);
    }

    [TestMethod]
    public void GetCurrentBallStates_Should_Return_AllBalls()
    {
        // Arrange
        var repo = new TestBallRepository();
        repo.Balls.Add(new Ball());
        repo.Balls.Add(new Ball());
        var logic = new BallLogic(repo, 800, 600);

        // Act
        var result = logic.GetCurrentBallStates();

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void DeleteBall_Should_Remove_FirstBall()
    {
        // Arrange
        var repo = new TestBallRepository();
        var ball1 = new Ball();
        var ball2 = new Ball();
        repo.Balls.Add(ball1);
        repo.Balls.Add(ball2);
        var logic = new BallLogic(repo, 800, 600);

        // Act
        logic.DeleteBall();

        // Assert
        Assert.AreEqual(1, repo.Balls.Count);
        Assert.AreSame(ball2, repo.Balls[0]);
    }

    [TestMethod]
    public void UpdateFrameSize_Should_Update_Internal_Dimensions()
    {
        // Arrange
        var logic = new BallLogic(new TestBallRepository(), 100, 100);

        // Act
        logic.updateFrameSize(1024, 768);

        // Sprawdzamy przez odbicie (np. pozycja kuli się zmieni po odbiciu od nowej krawędzi)
        var repo = new TestBallRepository();
        var ball = new Ball { x = 1010, y = 750, radius = 20, SpeedX = 10, SpeedY = 10 };
        repo.Balls.Add(ball);
        logic = new BallLogic(repo, 1024, 768);
        logic.UpdateBallPositions();

        Assert.IsTrue(ball.x <= 1024 - ball.radius);
        Assert.IsTrue(ball.y <= 768 - ball.radius);
    }

    #region Test Double for IBallRepository

    private class TestBallRepository : IBallRepository
    {
        public List<Ball> Balls { get; } = new();

        public void AddBall(Ball b) => Balls.Add(b);

        public IEnumerable<Ball> GetAllBalls() => Balls;

        public void removeBall(Ball b) => Balls.Remove(b);
    }

    #endregion
}
