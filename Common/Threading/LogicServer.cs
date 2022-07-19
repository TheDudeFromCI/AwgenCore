using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace AwgenCore
{
  public class LogicServer
  {
    private readonly ConcurrentQueue<RenderingTask> renderingTasks = new ConcurrentQueue<RenderingTask>();
    private readonly ConcurrentQueue<LogicTask> logicTasks = new ConcurrentQueue<LogicTask>();
    private readonly ConcurrentQueue<WorkerTask> queuedWorkerTasks = new ConcurrentQueue<WorkerTask>();
    private readonly BlockingCollection<WorkerTask> activeWorkerTasks = new BlockingCollection<WorkerTask>();
    private readonly List<Thread> workerThreads = new List<Thread>();
    private readonly List<LogicTask> logicUpdates = new List<LogicTask>();
    private readonly object activeTasksLock = new object();
    private readonly Thread logicThread;
    private readonly Thread renderThread;
    private readonly int targetFps;
    private bool running = true;
    private int activeTasks = 0;


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
    /// Checks whether or not the current thread is the logic thread.
    /// </summary>
    public bool IsLogicThread { get => Thread.CurrentThread == this.logicThread; }


    /// <summary>
    /// Checks whether or not the current thread is the render thread.
    /// </summary>
    public bool IsRenderThread { get => Thread.CurrentThread == this.renderThread; }


    /// <summary>
    /// Checks whether or not the current thread is a worker thread.
    /// </summary>
    public bool IsWorkerThread { get => !IsLogicThread && !IsRenderThread; }


    /// <summary>
    /// Gets the current number of worker threads being managed by this logic
    /// server.
    /// </summary>
    public int WorkerThreadCount { get => this.activeWorkerTasks.Count; }


    /// <summary>
    /// Creates and starts a new LogicThread instance.
    /// </summary>
    /// <param name="targetFps">The target number of update ticks per second.</param>
    /// <param name="workers">The number of worker threads to manage.</param>
    /// <exception cref="ArgumentException">If the targetFps is less than 1.</exception>
    public LogicServer(int targetFps, int workers)
    {
      if (targetFps <= 0) throw new ArgumentException("Target tick rate cannot be below 1!", nameof(targetFps));
      if (workers <= 0) throw new ArgumentException("Worker count cannot be below 1!", nameof(workers));

      this.targetFps = targetFps;
      this.renderThread = Thread.CurrentThread;

      for (var i = 0; i < workers; i++)
      {
        var workerThread = new Thread(WorkerThreadLoop);
        this.workerThreads.Add(workerThread);
        workerThread.Name = $"Awgen Worker {i}";
        workerThread.IsBackground = true;
        workerThread.Start();
      }

      this.logicThread = new Thread(Run);
      this.logicThread.Name = "Awgen Logic Server";
      this.logicThread.IsBackground = true;
      this.logicThread.Start();
    }


    /// <summary>
    /// Stops the logic thread and waits for it to finish executing.
    /// </summary>
    public void Stop()
    {
      this.running = false;
      this.logicThread.Join();
      foreach (var worker in this.workerThreads) worker.Join();
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
        timeAcc += endTime - frameTime;
        frameTime = endTime;

        var extraMs = (int)(timeDelta - timeAcc);
        if (extraMs >= 0) Thread.Sleep(extraMs);
      }
    }


    /// <summary>
    /// Executes all updates on the logic thread.
    /// </summary>
    private void Update()
    {
      ExecuteWorkerTasks();
      ExecuteLogicUpdates();
      ExecuteLogicTasks();
    }


    private void ExecuteWorkerTasks()
    {
      WorkerTask task;
      while (this.queuedWorkerTasks.TryDequeue(out task))
      {
        this.activeWorkerTasks.Add(task);
        Interlocked.Increment(ref this.activeTasks);
      }

      lock (this.activeTasksLock)
      {
        while (this.activeTasks > 0)
          Monitor.Wait(this.activeTasksLock);
      }
    }


    /// <summary>
    /// Causes all logic updates to be executed.
    /// </summary>
    private void ExecuteLogicUpdates()
    {
      lock (this.logicUpdates)
      {
        foreach (var update in this.logicUpdates)
          update.Execute();
      }
    }


    /// <summary>
    /// Causes all queued logic tasks currently within the queue to be executed.
    /// </summary>
    private void ExecuteLogicTasks()
    {
      LogicTask task;
      while (this.logicTasks.TryDequeue(out task))
        task.Execute();
    }


    /// <summary>
    /// Causes all queued rendering tasks currently within the queue to be
    /// executed.
    /// </summary>
    /// <param name="maxTasks">The maximum number of tasks that can be executed on this frame.</param>
    public void SyncRendering(int maxTasks = int.MaxValue)
    {
      var tasksExecuted = 0;
      RenderingTask task;
      while (this.renderingTasks.TryDequeue(out task) && tasksExecuted++ < maxTasks)
        task.Execute();
    }


    /// <summary>
    /// The thread loop used by all worker threads.
    /// </summary>
    private void WorkerThreadLoop()
    {
      while (this.running)
      {
        WorkerTask task;
        while (this.activeWorkerTasks.TryTake(out task, 1000))
        {
          task.Execute();
          if (task.FinalizedTask != null) this.logicTasks.Enqueue(task.FinalizedTask);
          Interlocked.Decrement(ref this.activeTasks);
          lock (this.activeTasksLock)
          {
            Monitor.Pulse(this.activeTasksLock);
          }
        }
      }
    }


    /// <summary>
    /// Adds a new rendering task to the queue. This will be executed the next
    /// time the rendering thread is synchronized with the logic server.
    /// </summary>
    /// <param name="task">The task to add.</param>
    public void QueueRenderingTask(RenderingTask task)
    {
      this.renderingTasks.Enqueue(task);
    }


    /// <summary>
    /// Adds a new logic task to the queue. If this is called from the rendering
    /// thread, this task will be executed on the very next game tick. If this
    /// is called from either the logic thread or a worker thread, then this
    /// task will be appended to the end of the current game tick.
    /// </summary>
    /// <param name="task">The task to add.</param>
    public void QueueLogicTask(LogicTask task)
    {
      this.logicTasks.Enqueue(task);
    }


    /// <summary>
    /// Adds a new worker task to the queue. This will be executed on the very
    /// next game tick before logic thread tasks are executed. This method may
    /// only be called from the logic thread.
    /// </summary>
    /// <param name="task">The task to add.</param>
    /// <exception cref="InvalidOperationException">If this method is not called from the logic thread.</exception>
    public void QueueWorkerTask(WorkerTask task)
    {
      if (!IsLogicThread) throw new InvalidOperationException("Worker tasks may only be triggered from the Logic Thread!");
      this.queuedWorkerTasks.Enqueue(task);
    }


    /// <summary>
    /// Adds a new logic update to the logic thread. These act similar to
    /// standard logic tasks, with the exception that they are not removed after
    /// each frame. These updates are registered to be executed before any other
    /// logic tasks and will always be executed every game time without needing
    /// to be re-added to the queue. This method may not be called from within
    /// the logic thread due to obvious concurrent modification issues.
    /// </summary>
    /// <param name="task">The task to add.</param>
    /// <exception cref="InvalidOperationException">If this method is called from the logic thread.</exception>
    public void AddLogicUpdate(LogicTask task)
    {
      if (IsLogicThread) throw new InvalidOperationException("Logic updates may not be added from within the logic thread!");
      lock (this.logicUpdates)
      {
        this.logicUpdates.Add(task);
      }
    }
  }
}
