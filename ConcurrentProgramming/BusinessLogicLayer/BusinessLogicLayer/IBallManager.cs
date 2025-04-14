using System.Collections.Generic;

public interface IBallManager
{
    void CreateBall();

    IEnumerable<DataLayer.Ball> GetCurrentBallStates();

    void UpdateBallPositions();

    void DeleteBall();

    void updateFrameSize(double newWidth, double newHeight);
}
