namespace AwgenCore.Voxel
{
  /// <summary>
  /// A 16x16x16 collection of blocks.
  /// </summary>
  public class Chunk
  {
    private readonly BlockPos chunkPos;
    private readonly Block[] blocks = new Block[16 * 16 * 16];


    /// <summary>
    /// Creates a new Chunk instance.
    /// </summary>
    /// <param name="chunkPos">The block position of the min corner of this chunk.</param>
    internal Chunk(BlockPos chunkPos)
    {
      this.chunkPos = chunkPos;

      var minPos = new BlockPos(0, 0, 0);
      var maxPos = new BlockPos(15, 15, 15);
      foreach (var pos in CuboidIterator.FromTwoCorners(minPos, maxPos))
      {
        int index = pos.x * 256 + pos.y * 16 + pos.z;
        this.blocks[index] = new Block(pos + chunkPos, BlockRegistry.VOID_BLOCK);
      }
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
  }
}
