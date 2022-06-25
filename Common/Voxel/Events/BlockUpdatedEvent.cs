namespace AwgenCore.Voxel
{
  /// <summary>
  /// Called whenever a block is updated. This is not called when a chunk is
  /// loaded or generated for the first time, only updates after the chunk
  /// already exists.
  /// </summary>
  public class BlockUpdatedEvent
  {
    private readonly Block block;


    /// <summary>
    /// Creates a new BlockUpdatedEvent instance.
    /// </summary>
    /// <param name="block">The block that is being updated.</param>
    public BlockUpdatedEvent(Block block)
    {
      this.block = block;
    }


    /// <summary>
    /// Gets the block that is being updated.
    /// </summary>
    /// <returns>The block.</returns>
    public Block GetBlock()
    {
      return this.block;
    }
  }
}
