using System;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A 16x16x16 collection of blocks.
  /// </summary>
  public class Chunk
  {
    private readonly Block[] blocks = new Block[16 * 16 * 16];
    private readonly World world;
    private readonly BlockPos chunkPos;


    /// <summary>
    /// Creates a new Chunk instance.
    /// </summary>
    /// <param name="world">The world that this chunk is in.</param>
    /// <param name="chunkPos">The block position of the min corner of this chunk.</param>
    internal Chunk(World world, BlockPos chunkPos)
    {
      if (world == null) throw new ArgumentNullException(nameof(world));

      this.world = world;
      this.chunkPos = chunkPos;

      foreach (var pos in CuboidIterator.OverChunk())
      {
        int index = pos.x * 256 + pos.y * 16 + pos.z;
        this.blocks[index] = new Block(this, pos + chunkPos, BlockRegistry.VOID_BLOCK);
      }

      GetWorld().TriggerChunkLoadedEvent(new ChunkLoadedEvent(this));
    }


    /// <summary>
    /// Gets the position of this chunk. The position is defined at the block
    /// position of the minimum corner of this chunk boundries.
    /// </summary>
    /// <returns>The region position.</returns>
    public BlockPos GetChunkPosition()
    {
      return this.chunkPos;
    }


    /// <summary>
    /// Gets the block at the specified block position within this chunk. If the
    /// block position is outside of the chunk, the coordinates are wrapped.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <returns>The block at the specified coordinates.</returns>
    public Block GetBlock(BlockPos pos)
    {
      pos &= 15;
      var index = pos.x * 256 + pos.y * 16 + pos.z;
      return this.blocks[index];
    }


    /// <summary>
    /// Gets the world that this chunk is in.
    /// </summary>
    /// <returns>The world.</returns>
    public World GetWorld()
    {
      return this.world;
    }
  }
}
