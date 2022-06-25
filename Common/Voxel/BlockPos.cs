using System;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// Represents a block position in world coordinates.
  /// </summary>
  public struct BlockPos
  {
    public int x;
    public int y;
    public int z;


    /// <summary>
    /// Creates a new block posiiton instance at the given coordinates.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    public BlockPos(int x, int y, int z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }


    /// <summary>
    /// Creates a new block position using the smallest value from each
    /// coordinate axis.
    /// </summary>
    /// <param name="a">The first block position.</param>
    /// <param name="b">The second block position.</param>
    /// <returns>The new block position.</returns>
    public static BlockPos Min(BlockPos a, BlockPos b)
    {
      return new BlockPos(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
    }


    /// <summary>
    /// Creates a new block position using the largest value from each
    /// coordinate axis.
    /// </summary>
    /// <param name="a">The first block position.</param>
    /// <param name="b">The second block position.</param>
    /// <returns>The new block position.</returns>
    public static BlockPos Max(BlockPos a, BlockPos b)
    {
      return new BlockPos(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
    }


    /// <summary>
    /// Applies the '&' bitwise operator to each axis coordinate.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <param name="val">The mask value.</param>
    /// <returns>The new block position.</returns>
    public static BlockPos operator &(BlockPos pos, int val)
    {
      return new BlockPos(pos.x & val, pos.y & val, pos.z & val);
    }


    /// <summary>
    /// Applies the '>>' bitwise operator to each axis coordinate.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <param name="val">The bit shift value.</param>
    /// <returns>The new block position.</returns>
    public static BlockPos operator >>(BlockPos pos, int val)
    {
      return new BlockPos(pos.x >> val, pos.y >> val, pos.z >> val);
    }


    /// <summary>
    /// Applies the '<<' bitwise operator to each axis coordinate.
    /// </summary>
    /// <param name="pos">The block position.</param>
    /// <param name="val">The bit shift value.</param>
    /// <returns>The new block position.</returns>
    public static BlockPos operator <<(BlockPos pos, int val)
    {
      return new BlockPos(pos.x << val, pos.y << val, pos.z << val);
    }


    /// <summary>
    /// Adds together each axis coordinate from two block positions.
    /// </summary>
    /// <param name="pos">The first block position.</param>
    /// <param name="val">The second block position.</param>
    /// <returns>The new block position.</returns>
    public static BlockPos operator +(BlockPos a, BlockPos b)
    {
      return new BlockPos(a.x + b.x, a.y + b.y, a.z + b.z);
    }
  }
}
