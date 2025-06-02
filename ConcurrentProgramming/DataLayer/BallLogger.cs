using DataLayer;
using System.Collections.Concurrent;

public class BallLogger : IDisposable
{
    private readonly string filePath;
    private readonly StreamWriter writer;
    private readonly Thread loggingThread;
    private readonly ConcurrentQueue<Ball> queue = new();
    private bool running = true;

    public BallLogger(string filePath)
    {
        this.filePath = filePath;
        writer = new StreamWriter(filePath, append: true) { AutoFlush = true };
        loggingThread = new Thread(ProcessQueue);
        loggingThread.IsBackground = true;
        loggingThread.Start();
    }

    public void Log(Ball ball)
    {
        queue.Enqueue(ball);
    }

    private void ProcessQueue()
    {
        while (running || !queue.IsEmpty)
        {
            if (queue.TryDequeue(out var ball))
            {
                // formatowanie linii
                string line = $"{DateTime.Now:O} Ball X={ball.x} Y={ball.y} SpeedX={ball.SpeedX} SpeedY={ball.SpeedY}";
                writer.WriteLine(line);
            }
            else
            {
                Thread.Sleep(50); // unikamy spinlocka
            }
        }
    }

    public void Stop()
    {
        running = false;
        loggingThread.Join();
        writer.Dispose();
    }

    public void Dispose()
    {
        Stop();
    }
}
