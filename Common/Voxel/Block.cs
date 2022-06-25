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
    private readonly Chunk chunk;
    private readonly BlockPos position;
    private BlockType blockType;


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

      this.chunk = chunk;
      this.position = position;
      this.blockType = blockType;
      ResetProperties();
    }


    /// <summary>
    /// Gets the position of this block.
    /// </summary>
    /// <returns>The block position.</returns>
    public BlockPos GetPosition()
    {
      return this.position;
    }


    /// <summary>
    /// Gets the current type of this block.
    /// </summary>
    /// <returns>The block type.</returns>
    public BlockType GetBlockType()
    {
      return this.blockType;
    }


    /// <summary>
    /// Updates the current type for this block. This will reset all current
    /// properties for this block to their default values based on the new block
    /// type.
    /// </summary>
    /// <param name="blockType">The new block type.</param>
    /// <exception cref="ArgumentNullException">If the block type is null.</exception>
    public void SetBlockType(BlockType blockType)
    {
      if (blockType == null) throw new ArgumentNullException(nameof(blockType));

      this.blockType = blockType;
      GetWorld().TriggerBlockUpdatedEvent(new BlockUpdatedEvent(this));
      ResetProperties();
    }


    /// <summary>
    /// Resets all properties for this block to their default value as specified
    /// by the block type.
    /// </summary>
    public void ResetProperties()
    {
      this.properties.Clear();
      foreach (var prop in this.blockType.GetDefaultProperties())
      {
        this.properties[prop.Key] = prop.Value;
      }
    }


    /// <summary>
    /// Gets the current value of a property on this block.
    /// </summary>
    /// <param name="property">The property to get.</param>
    /// <returns>The property value, or null if the property does not exist.</returns>
    public string GetProperty(string property)
    {
      return this.properties[property];
    }


    /// <summary>
    /// Updates a property in this block to a new value. The property must be
    /// defined within the block type that is currently assigned to this block
    /// in order to be considered valid.
    /// </summary>
    /// <param name="property">The property to update.</param>
    /// <param name="value">The new property value.</param>
    public void SetProperty(string property, string value)
    {
      if (!this.blockType.GetDefaultProperties().ContainsKey(property))
      {
        throw new ArgumentException($"'{nameof(property)}' is not a valid property for the block type: {this.blockType.GetResourceLocation()}.", nameof(property));
      }

      this.properties[property] = value;
    }


    /// <summary>
    /// Gets the chunk that this block is in.
    /// </summary>
    /// <returns>The chunk.</returns>
    public Chunk GetChunk()
    {
      return this.chunk;
    }


    /// <summary>
    /// Gets the world that this block is in.
    /// </summary>
    /// <returns>The world.</returns>
    public World GetWorld()
    {
      return this.chunk.GetWorld();
    }
  }
}
