namespace AwgenCore.Voxel
{
  /// <summary>
  /// A 16x1q6x16 collection of chunks.
  /// </summary>
  internal class Region
  {
    private readonly BlockPos regionPos;
    private readonly Chunk[] chunks = new Chunk[16 * 16 * 16];


    /// <summary>
    /// Creates a new Region instance.
    /// </summary>
    /// <param name="regionPos">The block position of the min corner of this region.</param>
    internal Region(BlockPos regionPos)
    {
      this.regionPos = regionPos;
    }


    /// <summary>
    /// Gets the position of this region. The position is defined at the block
    /// position of the minimum corner of this region boundries.
    /// </summary>
    /// <returns>The region position.</returns>
    internal BlockPos GetRegionPosition()
    {
      return this.regionPos;
    }


    /// <summary>
    /// Gets the chunk within this region that contains the given block position.
    /// </summary>
    /// <param name="blockPos">The block position.</param>
    /// <param name="create">Whether or not to create the chunk if it doesn't already exist.</param>
    /// <returns>The chunk, or null if the chunk does not exist.</returns>
    internal Chunk GetChunk(BlockPos blockPos, bool create)
    {
      blockPos &= 255;
      blockPos >>= 4;
      int index = blockPos.x * 256 + blockPos.y * 16 + blockPos.z;

      if (this.chunks[index] != null) return this.chunks[index];
      if (!create) return null;

      this.chunks[index] = new Chunk(blockPos << 4);
      return this.chunks[index];
    }
  }
}
