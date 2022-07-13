using AwgenCore;

namespace AwgenCore.Voxel
{
  public class ChunkLoadingTask : LogicTask
  {
    private readonly World world;

    public ChunkLoadingAnchor Anchor { get; private set; }


    public ChunkLoadingTask(LogicServer logicServer, World world, ChunkLoadingAnchor anchor) : base(logicServer)
    {
      this.world = world;
      Anchor = anchor;
    }

    protected override void ExecuteImpl()
    {
      BlockPos chunkPos;
      while (Anchor.NextChunk(out chunkPos))
      {
        if (this.world.GetChunk(chunkPos, false) == null)
        {
          this.world.GetChunk(chunkPos, true);
          return;
        }
      }
    }
  }
}
