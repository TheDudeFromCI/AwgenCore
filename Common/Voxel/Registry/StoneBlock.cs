// TODO Remove this object. This is only a temporary block type.

using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A special kind of "air" block that is generated by default into newly
  /// created chunks.
  /// </summary>
  public class StoneBlock : BlockType
  {
    private readonly MeshData meshData;


    /// <summary>
    /// Creates a new stone block instance.
    /// </summary>
    /// <param name="meshData">The mesh data for this block type.</param>
    public StoneBlock(MeshData meshData) : base(new ResourceLocation<BlockType>("awgen", "", "stone"))
    {
      this.meshData = meshData;
    }


    /// <inheritdoc/>
    public override MeshData GetMeshData(Dictionary<string, string> properties)
    {
      return this.meshData;
    }
  }
}
