using System;

public interface IBallManager
{
	void CreateBalls(int count);

    IEnumerable<BallData> GetBalls();

    void updatePositions();
}
