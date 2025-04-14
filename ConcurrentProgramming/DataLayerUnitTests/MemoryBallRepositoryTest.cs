using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System.Linq;

namespace DataLayer.Test
{
    [TestClass]
    public class MemoryBallRepositoryTests
    {
        [TestMethod]
        public void AddBall_ShouldIncreaseCount()
        {
            // Arrange
            var repo = new MemoryBallRepository();
            var ball = new Ball { x = 0, y = 0, radius = 10 };

            // Act
            repo.AddBall(ball);
            var allBalls = repo.GetAllBalls();

            // Assert
            Assert.AreEqual(1, allBalls.Count());
            Assert.AreSame(ball, allBalls.First());
        }

        [TestMethod]
        public void RemoveBall_ShouldDecreaseCount()
        {
            // Arrange
            var repo = new MemoryBallRepository();
            var ball1 = new Ball { x = 0, y = 0, radius = 10 };
            var ball2 = new Ball { x = 1, y = 1, radius = 15 };
            repo.AddBall(ball1);
            repo.AddBall(ball2);

            // Act
            repo.removeBall(ball1);
            var allBalls = repo.GetAllBalls();

            // Assert
            Assert.AreEqual(1, allBalls.Count());
            Assert.AreSame(ball2, allBalls.First());
        }

        [TestMethod]
        public void GetAllBalls_ShouldReturnEmptyList_WhenNoBalls()
        {
            // Arrange
            var repo = new MemoryBallRepository();

            // Act
            var allBalls = repo.GetAllBalls();

            // Assert
            Assert.AreEqual(0, allBalls.Count());
        }
    }
}
