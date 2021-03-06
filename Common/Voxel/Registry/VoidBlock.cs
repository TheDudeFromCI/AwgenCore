using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A special kind of "air" block that is generated by default into newly
  /// created chunks.
  /// </summary>
  public class VoidBlock : BlockType
  {
    private readonly MeshData emptyMeshData = new MeshData();


    /// <summary>
    /// Creates a new void block type instance.
    /// </summary>
    public VoidBlock() : base(new ResourceLocation<BlockType>("awgen", "", "void"))
    {
    }


    /// <inheritdoc/>
    public override bool OccludesSurface(Direction direction)
    {
      return false;
    }


    /// <inheritdoc/>
    public override MeshData GetMeshData(Dictionary<string, string> properties)
    {
      return this.emptyMeshData;
    }
  }
}
