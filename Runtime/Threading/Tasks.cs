using System;

namespace AwgenCore
{
  /// <summary>
  /// The base class for all thread task types.
  /// </summary>
  public abstract class ThreadTask
  {
    protected readonly LogicServer logicServer;

    /// <summary>
    /// Creates a new ThreadTask instance.
    /// </summary>
    /// <param name="logicServer">The logic server managing this task.</param>
    public ThreadTask(LogicServer logicServer)
    {
      this.logicServer = logicServer;
    }


    /// <summary>
    /// Causes this task to be executed.
    /// </summary>
    /// <exception cref="InvalidOperationException">If this task is executed on an invalid thread.</exception>
    public abstract void Execute();


    /// <summary>
    /// The task execution implementation. This is the task code itself that is
    /// executed after all thread-safety checks have been performed.
    /// </summary>
    protected abstract void ExecuteImpl();
  }


  /// <summary>
  /// A task that is created on the logic thread in order to be executed on the
  /// rendering thread.
  /// </summary>
  public abstract class RenderingTask : ThreadTask
  {
    /// <summary>
    /// Creates a new RenderingTask instance.
    /// </summary>
    /// <param name="logicServer">The logic server managing this task.</param>
    public RenderingTask(LogicServer logicServer) : base(logicServer)
    {
    }


    /// <inheritdoc/>
    public override void Execute()
    {
      if (!this.logicServer.IsRenderThread) throw new InvalidOperationException("Task may only be executed on the Rendering Thread!");
      ExecuteImpl();
    }
  }


  /// <summary>
  /// A task that is created on the render thread in order to be executed on the
  /// logic thread.
  /// </summary>
  public abstract class LogicTask : ThreadTask
  {
    /// <summary>
    /// Creates a new LogicTask instance.
    /// </summary>
    /// <param name="logicServer">The logic server managing this task.</param>
    public LogicTask(LogicServer logicServer) : base(logicServer)
    {
    }


    /// <inheritdoc/>
    public override void Execute()
    {
      if (!this.logicServer.IsLogicThread) throw new InvalidOperationException("Task may only be executed on the Logic Thread!");
      ExecuteImpl();
    }
  }


  /// <summary>
  /// A task that is created on the logic thread in order to be executed on a
  /// worker thread. This task should be read-only and only calculate data to
  /// be finalized using a logic task.
  /// </summary>
  public abstract class WorkerTask : ThreadTask
  {
    /// <summary>
    /// A task that may or may not be generated after this task has finished in
    /// order to be executed on the logic thread in order to finalize any data
    /// that has been calculated by this worker task.
    /// </summary>
    public LogicTask FinalizedTask { get; protected set; }


    /// <summary>
    /// Creates a new WorkerTask instance.
    /// </summary>
    /// <param name="logicServer">The logic server managing this task.</param>
    public WorkerTask(LogicServer logicServer) : base(logicServer)
    {
    }


    /// <inheritdoc/>
    public override void Execute()
    {
      if (!this.logicServer.IsWorkerThread) throw new InvalidOperationException("Task may only be executed on a Worker Thread!");
      ExecuteImpl();
    }
  }
}
