using DataLayer;
using System.Collections.Concurrent;

public class BallLogger : IDisposable
{
    private readonly string filePath;
    private readonly StreamWriter writer;
    private readonly Thread loggingThread;
    private readonly ConcurrentQueue<Ball> queue = new();
    private volatile bool running = true;
    private bool disposed = false;

    public BallLogger(string filePath)
    {
        this.filePath = filePath;
        var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        writer = new StreamWriter(fs) { AutoFlush = true };
        loggingThread = new Thread(ProcessQueue)
        {
            IsBackground = true
        };
        loggingThread.Start();
    }

    public void Log(Ball ball)
    {
        if (!running || disposed)
            throw new ObjectDisposedException(nameof(BallLogger));
        queue.Enqueue(ball);
    }

    private void ProcessQueue()
    {
        while (running || !queue.IsEmpty)
        {
            try
            {
                if (queue.TryDequeue(out var ball))
                {
                    string line = $"{DateTime.Now:O} Ball X={ball.x} Y={ball.y} SpeedX={ball.SpeedX} SpeedY={ball.SpeedY}";
                    writer.WriteLine(line);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Logger] Write error: {ex.Message}");
            }
        }
    }

    public void Stop()
    {
        running = false;
        loggingThread.Join(); 
        writer.Flush();       // pewność, że bufor się opróżnił
        writer.Dispose();     // dopiero teraz zamykamy
    }
    
    public void Dispose()
    {
        if (disposed) return;
        Stop();
        disposed = true;
        GC.SuppressFinalize(this);
    }
}
