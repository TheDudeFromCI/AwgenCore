namespace AwgenCore
{
  /// <summary>
  /// A task that is created on the logic thread in order to be executed on the
  /// rendering thread.
  /// </summary>
  public interface IRenderingTask
  {
    /// <summary>
    /// Causes this task to be executed.
    /// </summary>
    void Execute();
  }
}
