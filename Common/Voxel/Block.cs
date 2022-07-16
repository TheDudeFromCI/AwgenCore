using System;
using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A block of data contained within a cubic voxel in a voxelized environment.
  /// </summary>
  public class Block
  {
    private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
    private BlockType blockType;


    /// <summary>
    /// Gets the chunk that this block was created in.
    /// </summary>
    public readonly Chunk Chunk;


    /// <summary>
    /// Gets the position of this block in world coordinates.
    /// </summary>
    public readonly BlockPos Position;


    /// <summary>
    /// Gets or sets a given property for this block based on it's block type.
    /// If this block does not specifically override a property, the default
    /// property value is returned.
    /// </summary>
    /// <param name="property">The property name to get or set.</param>
    /// <returns>The value of this property for the block type.</returns>
    /// <exception cref="ArgumentException">If the property does not exist for this block type.</exception>
    /// <exception cref="ArgumentNullException">If the property name or value is null.</exception>
    public string this[string property]
    {
      get
      {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (this.properties.ContainsKey(property)) return this.properties[property];
        if (this.blockType.DefaultProperties.ContainsKey(property)) return this.blockType.DefaultProperties[property];
        return null;
      }

      set
      {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (value == null) throw new ArgumentNullException(nameof(value));

        if (!this.blockType.DefaultProperties.ContainsKey(property))
          throw new ArgumentException($"{property} is not a valid property for the current block type: {this.blockType}", nameof(property));

        if (this.blockType.DefaultProperties[property].Equals(value)) this.properties.Remove(property);
        else this.properties[property] = value;
      }
    }


    /// <summary>
    /// Gets or sets the current type for this block. Assigning a new block type
    /// to this block will reset all properties to their default values based
    /// off the new block type.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the block type is null.</exception>
    public BlockType BlockType
    {
      get => this.blockType;
      set
      {
        if (value == null) throw new ArgumentNullException(nameof(value));
        this.blockType = value;
        this.properties.Clear();
        World.TriggerBlockUpdatedEvent(new BlockUpdatedEvent(this));
      }
    }


    /// <summary>
    /// Gets the world that this block is in.
    /// </summary>
    public World World { get => Chunk.World; }


    /// <summary>
    /// Creates a new Block instance.
    /// </summary>
    /// <param name="chunk">The chunk that this block is in.</param>
    /// <param name="position">The position of this block instance.</param>
    /// <param name="blockType">The type of this block.</param>
    /// <exception cref="ArgumentNullException">If the block type is null.</exception>
    internal Block(Chunk chunk, BlockPos position, BlockType blockType)
    {
      if (chunk == null) throw new ArgumentNullException(nameof(chunk));
      if (blockType == null) throw new ArgumentNullException(nameof(blockType));

      Chunk = chunk;
      Position = position;
      BlockType = blockType;
    }


    /// <summary>
    /// Sets this block type and corresponding properties based on it's
    /// qualified name.
    /// </summary>
    /// <param name="qualifiedName">The qualified block name.</param>
    public void SetTypeAndProperties(QualifiedName<BlockType> qualifiedName)
    {
      if (qualifiedName == null) throw new ArgumentNullException(nameof(qualifiedName));

      BlockType = qualifiedName.RegisterableInstance;
      foreach (var pair in qualifiedName.Properties) this[pair.Key] = pair.Value;
    }


    /// <summary>
    /// Resets all properties for this block to their default value as specified
    /// by the block type.
    /// </summary>
    public void ResetProperties()
    {
      this.properties.Clear();
    }
  }
}
