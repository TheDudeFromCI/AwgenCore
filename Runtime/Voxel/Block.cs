namespace AwgenCore.Voxel
{
  /// <summary>
  /// A block of data contained within a cubic voxel in a voxelized environment.
  /// </summary>
  public class Block
  {
    private readonly BlockPos position;


    /// <summary>
    /// Creates a new Block instance.
    /// </summary>
    /// <param name="position">The position of this block instance.</param>
    internal Block(BlockPos position)
    {
      this.position = position;
    }


    /// <summary>
    /// Gets the position of this block.
    /// </summary>
    /// <returns>The block position.</returns>
    public BlockPos GetPosition()
    {
      return this.position;
    }
  }
}
