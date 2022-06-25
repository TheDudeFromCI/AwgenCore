namespace AwgenCore.Voxel
{
  /// <summary>
  /// An event that is triggered whenever a new chunk is loaded.
  /// </summary>
  public class ChunkLoadedEvent
  {
    private readonly Chunk chunk;


    /// <summary>
    /// Creates a new ChunkLoadedEvent instance.
    /// </summary>
    /// <param name="chunk">The chunk that is being loaded.</param>
    public ChunkLoadedEvent(Chunk chunk)
    {
      this.chunk = chunk;
    }


    /// <summary>
    /// Gets the chunk that is being loaded.
    /// </summary>
    /// <returns>The chunk.</returns>
    public Chunk GetChunk()
    {
      return this.chunk;
    }
  }
}
