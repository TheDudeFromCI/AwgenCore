using System;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A 16x1q6x16 collection of chunks.
  /// </summary>
  internal class Region
  {
    private readonly Chunk[] chunks = new Chunk[16 * 16 * 16];


    /// <summary>
    /// Gets the position of this region. The position is defined at the block
    /// position of the minimum corner of this region boundries.
    /// </summary>
    public readonly BlockPos Position;


    /// <summary>
    /// Gets the world this region is in.
    /// </summary>
    public readonly World World;


    /// <summary>
    /// Creates a new Region instance.
    /// </summary>
    /// <param name="world">The world that this region is in.</param>
    /// <param name="regionPos">The block position of the min corner of this region.</param>
    internal Region(World world, BlockPos regionPos)
    {
      if (world == null) throw new ArgumentNullException(nameof(world));

      World = world;
      Position = regionPos;
    }


    /// <summary>
    /// Gets the chunk within this region that contains the given block position.
    /// </summary>
    /// <param name="blockPos">The block position.</param>
    /// <param name="create">Whether or not to create the chunk if it doesn't already exist.</param>
    /// <returns>The chunk, or null if the chunk does not exist.</returns>
    internal Chunk GetChunk(BlockPos blockPos, bool create)
    {
      var indexPos = (blockPos & 255) >> 4;
      int index = indexPos.x * 256 + indexPos.y * 16 + indexPos.z;

      if (this.chunks[index] != null) return this.chunks[index];
      if (!create) return null;

      this.chunks[index] = new Chunk(World, blockPos & ~15);
      return this.chunks[index];
    }
  }
}
