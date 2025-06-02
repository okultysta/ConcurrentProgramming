using DataLayer;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

public class BallLogger : IDisposable
{
    private readonly string logFilePath;
    private readonly BlockingCollection<string> logQueue = new();
    private readonly Thread loggingThread;
    private bool disposed = false;

    public BallLogger(string filePath)
    {
        logFilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        loggingThread = new Thread(ProcessQueue)
        {
            IsBackground = true
        };
        loggingThread.Start();
    }

    public void Log(Ball ball)
    {
        if (disposed) throw new ObjectDisposedException(nameof(BallLogger));
        if (ball == null) throw new ArgumentNullException(nameof(ball));

        string logLine = $"{DateTime.Now:o} Ball X={ball.x} Y={ball.y} SpeedX={ball.SpeedX} SpeedY={ball.SpeedY}";
        logQueue.Add(logLine);
    }

    private void ProcessQueue()
    {
        try
        {
            using StreamWriter writer = new(logFilePath, append: true);
            foreach (var line in logQueue.GetConsumingEnumerable())
            {
                writer.WriteLine(line);
                writer.Flush();
            }
        }
        catch (Exception ex)
        {
            // Log error lub obsłuż w inny sposób
            Console.Error.WriteLine("Logger error: " + ex.Message);
        }
    }

    // Kończy przyjmowanie logów i czeka na zakończenie wątku
    public void Stop()
    {
        if (disposed) return;

        logQueue.CompleteAdding();
        loggingThread.Join();
        disposed = true;
    }

    // Implementacja IDisposable
    public void Dispose()
    {
        Stop();
    }
}
