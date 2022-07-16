using System;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A 16x16x16 collection of blocks.
  /// </summary>
  public class Chunk
  {
    private readonly Block[] blocks = new Block[BLOCK_COUNT];

    /// <summary>
    /// The total number of blocks within a single chunk.
    /// </summary>
    public const int BLOCK_COUNT = 16 * 16 * 16;


    /// <summary>
    /// Gets the world that this chunk is in.
    /// </summary>
    public readonly World World;


    /// <summary>
    /// Gets the block position of this chunk. The position is defined at the
    /// block position of the minimum corner of this chunk boundries.
    /// </summary>
    public readonly BlockPos Position;


    /// <summary>
    /// Gets the block at the specified block position within this chunk. If the
    /// block position is outside of the chunk, the coordinates are wrapped.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <returns>The block at the specified coordinates.</returns>
    public Block this[BlockPos pos]
    {
      get
      {
        pos &= 15;
        var index = pos.x * 256 + pos.y * 16 + pos.z;
        return this.blocks[index];
      }
    }


    /// <summary>
    /// Gets the block at the specified block index within a 1D array.
    /// </summary>
    /// <param name="index">The local block index.</param>
    /// <returns>The block at the given index.</returns>
    public Block this[int index] { get => this.blocks[index]; }


    /// <summary>
    /// Creates a new Chunk instance.
    /// </summary>
    /// <param name="world">The world that this chunk is in.</param>
    /// <param name="chunkPos">The block position of the min corner of this chunk.</param>
    internal Chunk(World world, BlockPos chunkPos)
    {
      if (world == null) throw new ArgumentNullException(nameof(world));

      World = world;
      Position = chunkPos;

      foreach (var pos in CuboidIterator.OverChunk())
      {
        int index = pos.x * 256 + pos.y * 16 + pos.z;
        this.blocks[index] = new Block(this, pos + chunkPos, BlockRegistry.VOID_BLOCK);
      }

      World.TriggerChunkLoadedEvent(new ChunkLoadedEvent(this));
    }
  }
}
