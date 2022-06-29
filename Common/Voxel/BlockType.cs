using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A type of data that defines how a certain Block type is supposed to
  /// behave.
  /// </summary>
  public abstract class BlockType : IRegistrable<BlockType>
  {
    private readonly Dictionary<string, string> defaultProperties = new Dictionary<string, string>();


    /// <summary>
    /// Creates a new BlockType instance.
    /// </summary>
    /// <param name="name">The name of this block type.</param>
    /// <param name="classname">The namespace of this block type.</param>
    public BlockType(string name, string classname) : base(name, classname)
    {
    }


    /// <summary>
    /// Gets a list of properties that are used by this block type, as well as
    /// their corresponding default values.
    /// </summary>
    /// <returns>A dictionary of property/value pairs for all block state properties.</returns>
    public Dictionary<string, string> GetDefaultProperties()
    {
      return this.defaultProperties;
    }


    /// <summary>
    /// Creates a new property for this block type and sets it's default value.
    /// </summary>
    /// <param name="property">The property name.</param>
    /// <param name="value">The default property value.</param>
    protected void SetDefaultProperty(string property, string value)
    {
      this.defaultProperties[property] = value;
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
