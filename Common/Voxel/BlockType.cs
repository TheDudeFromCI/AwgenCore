namespace AwgenCore.Voxel
{
  /// <summary>
  /// A type of data that defines how a certain Block type is supposed to
  /// behave.
  /// </summary>
  public abstract class BlockType : IRegistrable<BlockType>
  {
    /// <summary>
    /// Creates a new BlockType instance.
    /// </summary>
    /// <param name="resource">The resource location of this block type.</param>
    public BlockType(ResourceLocation<BlockType> resource) : base(resource)
    {
    }

    /// <summary>
    /// Checks whether or not this block occludes another block's surface in a
    /// give direction relative to this block. A block that is completely solid
    /// will usually return true for all directions while a block that is
    /// invisible will return false for all directions. If a surface is only
    /// partially occluded, this function should still return false.
    /// </summary>
    /// <param name="direction">The direction of the surface of this block.</param>
    /// <returns>True if this block fully occludes blocks in the given direction. False otherwise.</returns>
    public virtual bool OccludesSurface(Direction direction)
    {
      return true;
    }
  }
}
