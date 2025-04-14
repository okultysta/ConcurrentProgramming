namespace DataLayer
{
    public interface IBallRepository
    {
        void AddBall(Ball ball);
        IEnumerable<Ball> GetAllBalls();

        void removeBall(Ball ball);
    }
}
