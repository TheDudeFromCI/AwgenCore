namespace AwgenCore.Voxel
{
  /// <summary>
  /// Wraps around a world object to listen for chunk updates and generates
  /// corresponding remesh tasks for those chunks.
  /// </summary>
  public class ChunkRemeshListener
  {
    private readonly LogicServer logicServer;
    private readonly IChunkMeshReciever meshReciever;
    private readonly IVoxelMesher voxelMesher;


    /// <summary>
    /// Creates a new ChunkRemeshListener.
    /// </summary>
    /// <param name="logicServer">The logic server to execute the tasks on.</param>
    /// <param name="world">The world to listen for updates on.</param>
    /// <param name="meshReciever">The reciever to handle completed chunk meshes.</param>
    /// <param name="voxelMesher">The handler that generates a mesh from a chunk.</param>
    public ChunkRemeshListener(LogicServer logicServer, World world, IChunkMeshReciever meshReciever, IVoxelMesher voxelMesher)
    {
      this.logicServer = logicServer;
      this.meshReciever = meshReciever;
      this.voxelMesher = voxelMesher;

      world.onChunkLoaded += OnChunkLoaded;
      world.onBlockUpdated += OnBlockUpdated;
    }


    /// <summary>
    /// An event that is called whenever a new chunk is loaded in the world.
    /// </summary>
    /// <param name="ev">The event.</param>
    private void OnChunkLoaded(ChunkLoadedEvent ev)
    {
      RegenerateChunk(ev.GetChunk());
    }


    /// <summary>
    /// An event that is called whenever a block is updated in the world.
    /// </summary>
    /// <param name="ev">The event.</param>
    private void OnBlockUpdated(BlockUpdatedEvent ev)
    {
      RegenerateChunk(ev.GetBlock().Chunk);
    }


    /// <summary>
    /// Creates and queues a remeshing worker task for the given chunk.
    /// </summary>
    /// <param name="chunk">The chunk to generate a mesh for.</param>
    private void RegenerateChunk(Chunk chunk)
    {
      var task = new ChunkRemeshTask(this.logicServer, this.meshReciever, this.voxelMesher, chunk);
      this.logicServer.QueueWorkerTask(task);
    }
  }
}
