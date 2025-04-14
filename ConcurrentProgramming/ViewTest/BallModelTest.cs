using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using View;

[TestClass]
public class BallModelTests
{
    [TestMethod]
    public void Default_Values_Should_Be_Correct()
    {
        // Arrange
        var model = new BallModel();

        // Assert
        Assert.AreEqual(0, model.X);
        Assert.AreEqual(0, model.Y);
        Assert.AreEqual(20, model.Radius); // domyślny promień
    }

    [TestMethod]
    public void Setting_X_Should_Trigger_PropertyChanged()
    {
        // Arrange
        var model = new BallModel();
        bool eventTriggered = false;
        model.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(model.X))
                eventTriggered = true;
        };

        // Act
        model.X = 100;

        // Assert
        Assert.IsTrue(eventTriggered);
        Assert.AreEqual(100, model.X);
    }

    [TestMethod]
    public void Setting_Y_Should_Trigger_PropertyChanged()
    {
        // Arrange
        var model = new BallModel();
        bool eventTriggered = false;
        model.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(model.Y))
                eventTriggered = true;
        };

        // Act
        model.Y = 200;

        // Assert
        Assert.IsTrue(eventTriggered);
        Assert.AreEqual(200, model.Y);
    }

    [TestMethod]
    public void Setting_Radius_Should_Trigger_PropertyChanged()
    {
        // Arrange
        var model = new BallModel();
        bool eventTriggered = false;
        model.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(model.Radius))
                eventTriggered = true;
        };

        // Act
        model.Radius = 30;

        // Assert
        Assert.IsTrue(eventTriggered);
        Assert.AreEqual(30, model.Radius);
    }
}
