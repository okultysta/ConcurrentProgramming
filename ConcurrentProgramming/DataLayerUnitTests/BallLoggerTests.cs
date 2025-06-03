using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            logger.Dispose();
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
            Thread.Sleep(150); // więcej czasu dla wątku loggera
            logger.Dispose();
            // Assert
            string[] lines = File.ReadAllLines(tempLogFile);
            Assert.AreEqual(1, lines.Length);

            var line = lines[0];
            Assert.IsTrue(line.Contains("X=123.45") || line.Contains("X=123,45"));
            Assert.IsTrue(line.Contains("Y=67.89") || line.Contains("Y=67,89"));
            Assert.IsTrue(line.Contains("SpeedX=-1.23") || line.Contains("SpeedX=-1,23"));
            Assert.IsTrue(line.Contains("SpeedY=4.56") || line.Contains("SpeedY=4,56"));

            DateTime parsedDate;
            Assert.IsTrue(DateTime.TryParse(line.Split(' ')[0], out parsedDate));
        }

       


    }
}
