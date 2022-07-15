// TODO Remove this object. This is only a temporary block type.

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A special kind of "air" block that is generated by default into newly
  /// created chunks.
  /// </summary>
  public class StoneBlock : BlockType
  {
    public StoneBlock() : base(new ResourceLocation<BlockType>("awgen", "", "stone"))
    {
    }
  }
}
