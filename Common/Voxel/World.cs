using System;
using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  public class World
  {
    private readonly Dictionary<BlockPos, Region> regions = new Dictionary<BlockPos, Region>();


    /// <summary>
    /// Gets the generator that is being used to populate new chunks.
    /// </summary>
    public readonly IWorldGenerator WorldGenerator;


    /// <summary>
    /// An event that is triggered whenever this world generates a new chunk.
    /// </summary>
    public event Action<ChunkLoadedEvent> onChunkLoaded;


    /// <summary>
    /// An event that is called whenever a block is updated. This does not
    /// trigger for blocks being created when a chunk is loaded.
    /// </summary>
    public event Action<BlockUpdatedEvent> onBlockUpdated;


    /// <summary>
    /// Creates a new World instance.
    /// </summary>
    /// <param name="worldGenerator">The generator to use for populating new chunks.</param>
    public World(IWorldGenerator worldGenerator)
    {
      WorldGenerator = worldGenerator;
    }


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

      Region region = new Region(this, pos);
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
      return chunk[pos];
    }


    /// <summary>
    /// Causes the ChunkLoadedEvent to be triggered for this world.
    /// </summary>
    /// <param name="ev">The event.</param>
    internal void TriggerChunkLoadedEvent(ChunkLoadedEvent ev)
    {
      this.onChunkLoaded(ev);
    }


    /// <summary>
    /// Causes the BlockUpdatedEvent to be triggered for this world.
    /// </summary>
    /// <param name="ev">The event.</param>
    internal void TriggerBlockUpdatedEvent(BlockUpdatedEvent ev)
    {
      this.onBlockUpdated(ev);
    }
  }
}
