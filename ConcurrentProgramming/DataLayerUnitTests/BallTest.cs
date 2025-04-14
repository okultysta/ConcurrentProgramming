using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;

[TestClass]
public class BallTests
{
    [TestMethod]
    public void Constructor_Should_InitializePropertiesCorrectly()
    {
        // Arrange
        double expectedX = 100;
        double expectedY = 150;
        int expectedRadius = 20;
        double expectedSpeedX = 2.5;
        double expectedSpeedY = -1.5;

        // Act
        var ball = new Ball(expectedX, expectedY, expectedRadius, expectedSpeedX, expectedSpeedY);

        // Assert
        Assert.AreEqual(expectedX, ball.x);
        Assert.AreEqual(expectedY, ball.y);
        Assert.AreEqual(expectedRadius, ball.radius);
        Assert.AreEqual(expectedSpeedX, ball.SpeedX);
        Assert.AreEqual(expectedSpeedY, ball.SpeedY);
    }

    [TestMethod]
    public void Properties_Should_Be_Settable()
    {
        // Arrange
        var ball = new Ball();

        // Act
        ball.x = 50;
        ball.y = 75;
        ball.radius = 10;
        ball.SpeedX = 1.0;
        ball.SpeedY = -1.0;

        // Assert
        Assert.AreEqual(50, ball.x);
        Assert.AreEqual(75, ball.y);
        Assert.AreEqual(10, ball.radius);
        Assert.AreEqual(1.0, ball.SpeedX);
        Assert.AreEqual(-1.0, ball.SpeedY);
    }

    [TestMethod]
    public void Move_Should_Update_Position_By_Speed()
    {
        // Arrange
        var ball = new Ball
        {
            x = 10,
            y = 20,
            SpeedX = 5,
            SpeedY = -3
        };

        // Act
        ball.Move();

        // Assert
        Assert.AreEqual(15, ball.x);
        Assert.AreEqual(17, ball.y);
    }

    [TestMethod]
    public void Move_Should_Be_Idempotent_If_Speed_Is_Zero()
    {
        // Arrange
        var ball = new Ball
        {
            x = 10,
            y = 20,
            SpeedX = 0,
            SpeedY = 0
        };

        // Act
        ball.Move();

        // Assert
        Assert.AreEqual(10, ball.x);
        Assert.AreEqual(20, ball.y);
    }
}
