namespace AwgenCore.Voxel
{
  /// <summary>
  /// A generator that can be used for generating chunks when they are loaded.
  /// </summary>
  public interface IWorldGenerator
  {
    /// <summary>
    /// Called whenever a new chunk is loaded in order to recieve the standard
    /// starting blocks.
    /// </summary>
    /// <param name="chunkPos">The position of the chunk in block coordinates.</param>
    /// <returns>An array of qualified block type names that define the contents of the chunk.</returns>
    QualifiedName<BlockType>[] GenerateChunk(BlockPos chunkPos);
  }
}
