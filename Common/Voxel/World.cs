using AwgenCore;
using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  public class World
  {
    private readonly Dictionary<BlockPos, Region> regions = new Dictionary<BlockPos, Region>();


    /// <summary>
    /// Gets the region that contains the given block position.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <param name="create">Whether or not to create the region if it does not currently exist.</param>
    /// <returns>The region that contains the block position, or null if it does not exist.</returns>
    private Region GetRegion(BlockPos pos, bool create)
    {
      pos &= ~255;

      if (this.regions.ContainsKey(pos)) return this.regions[pos];
      if (!create) return null;

      Region region = new Region(pos);
      this.regions[pos] = region;
      return region;
    }


    /// <summary>
    /// Gets the chunk that contains the given block position.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <param name="create">Whether or not to create the chunk if it does not currently exist.</param>
    /// <returns>The chunk that contains the block position, or null if it does not exist.</returns>
    public Chunk GetChunk(BlockPos pos, bool create)
    {
      var region = GetRegion(pos, create);
      if (region == null) return null;
      return region.GetChunk(pos, create);
    }


    /// <summary>
    /// Gets the block at the given block position.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <param name="create">Whether or not to create the block if it does not currently exist.</param>
    /// <returns>The block at the given block position, or null if it does not exist.</returns>
    public Block GetBlock(BlockPos pos, bool create)
    {
      Chunk chunk = GetChunk(pos, create);
      if (chunk == null) return null;
      return chunk.GetBlock(pos);
    }
  }
}
