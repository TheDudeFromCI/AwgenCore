namespace AwgenCore.Voxel
{
  /// <summary>
  /// A logic update loop task for triggering a world instance to continuously
  /// load essential chunks over time. This task will trigger one new chunk to
  /// load each game tick for a given chunk loading anchor. Chunks that are
  /// already loaded are skipped. This task will perform no action if the anchor
  /// has no more chunks ready to be loaded.
  /// </summary>
  public class ChunkLoadingTask : LogicTask
  {
    private readonly World world;


    /// <summary>
    /// Gets the ChunkLoadingAnchor that is being used to handle which chunks
    /// should be loaded.
    /// </summary>
    public ChunkLoadingAnchor Anchor { get; private set; }


    /// <summary>
    /// Creates a new ChunkLoadingTask instance.
    /// </summary>
    /// <param name="logicServer">The logic server this task is being run on.</param>
    /// <param name="world">The world to load the chunks into.</param>
    /// <param name="anchor">The chunk anchor to decide which chunks to load.</param>
    public ChunkLoadingTask(LogicServer logicServer, World world, ChunkLoadingAnchor anchor) : base(logicServer)
    {
      this.world = world;
      Anchor = anchor;
    }


    /// <inheritdoc/>
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
