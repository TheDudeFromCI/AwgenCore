using System;
using System.Threading;
using System.Collections.Concurrent;

namespace AwgenCore
{
  public class LogicThread
  {
    private readonly ConcurrentQueue<IRenderingTask> queue = new ConcurrentQueue<IRenderingTask>();
    private readonly Thread thread;
    private readonly int targetFps;
    private bool running = true;


    /// <summary>
    /// Gets the target number of update ticks that occur every second.
    /// </summary>
    public int UpdatesPerSecond { get => this.targetFps; }


    /// <summary>
    /// Gets the delta time, in seconds, that occur between each update frame.
    /// </summary>
    public float DeltaTime { get => 1000f / this.targetFps; }


    /// <summary>
    /// Checks whether or not this logic thread is still currently running.
    /// </summary>
    public bool IsRunning { get => this.running; }


    /// <summary>
    /// Creates and starts a new LogicThread instance.
    /// </summary>
    /// <param name="targetFps">The target number of update ticks per second.</param>
    /// <exception cref="ArgumentException">If the targetFps is less than 1.</exception>
    public LogicThread(int targetFps)
    {
      if (targetFps <= 0) throw new ArgumentException("Target tick rate cannot be below 1!", nameof(targetFps));

      this.targetFps = targetFps;

      this.thread = new Thread(Run);
      this.thread.Name = "Awgen Logic Server";
      this.thread.IsBackground = true;
      this.thread.Start();
    }


    /// <summary>
    /// Stops the logic thread and waits for it to finish executing.
    /// </summary>
    public void Stop()
    {
      this.running = false;
      this.thread.Join();
    }


    /// <summary>
    /// The logic thread update loop.
    /// </summary>
    private void Run()
    {
      var timeDelta = 1000.0 / this.targetFps;
      var timeAcc = 0.0;
      var frameTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

      while (this.running)
      {
        while (timeAcc >= timeDelta)
        {
          Update();
          timeAcc -= timeDelta;
        }

        var endTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        timeAcc += frameTime - endTime;
        frameTime = endTime;

        var extraMs = (int)((timeDelta - timeAcc) * 1000);
        if (extraMs >= 0) Thread.Sleep(extraMs);
      }
    }


    /// <summary>
    /// Executes all updates on the logic thread.
    /// </summary>
    private void Update()
    {
    }


    /// <summary>
    /// Causes all queued rendering tasks currently within the queue to be
    /// executed.
    /// </summary>
    public void SyncRendering()
    {
      IRenderingTask task;
      while (this.queue.TryDequeue(out task))
        task.Execute();
    }
  }
}
