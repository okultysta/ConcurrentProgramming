using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace DataLayer.Tests
{
    [TestClass]
    public class BallLoggerTests
    {
        private string tempLogFile;
        private BallLogger logger;

        [TestInitialize]
        public void Setup()
        {
            tempLogFile = Path.GetTempFileName();
            logger = new BallLogger(tempLogFile);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(tempLogFile))
                File.Delete(tempLogFile);
        }

        [TestMethod]
        public void LogPosition_WritesCorrectLineToFile()
        {
            // Arrange
            var ball = new Ball
            {
                x = 123.45,
                y = 67.89,
                SpeedX = -1.23,
                SpeedY = 4.56
            };

            // Act
            logger.Log(ball);
            Thread.Sleep(100); // poczekaj, żeby logger zdążył zapisać
            logger.Stop();

            // Assert
            string[] lines = File.ReadAllLines(tempLogFile);
            Assert.AreEqual(1, lines.Length);
            StringAssert.Contains(lines[0], "Ball X=123.45");
            StringAssert.Contains(lines[0], "Y=67.89");
            StringAssert.Contains(lines[0], "SpeedX=-1.23");
            StringAssert.Contains(lines[0], "SpeedY=4.56");

            DateTime parsedDate;
            Assert.IsTrue(DateTime.TryParse(lines[0].Split(' ')[0], out parsedDate));
        }


    }
}
