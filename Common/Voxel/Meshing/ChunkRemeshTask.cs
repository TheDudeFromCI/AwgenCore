using UnityEngine;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A worker task that regenerates a chunk mesh.
  /// </summary>
  public class ChunkRemeshTask : WorkerTask
  {
    private readonly IChunkMeshReciever meshReciever;
    private readonly IVoxelMesher voxelMesher;
    private readonly Chunk chunk;


    /// <summary>
    /// Creates a new ChunkRemeshTask instance.
    /// </summary>
    /// <param name="logicServer">The logic server this task is being executed on.</param>
    /// <param name="meshReciever">The handle to send the generated chunk mesh to.</param>
    /// <param name="mesher">The handler to use when generating the chunk mesh.</param>
    /// <param name="chunk">The chunk to generate a mesh for.</param>
    public ChunkRemeshTask(LogicServer logicServer, IChunkMeshReciever meshReciever, IVoxelMesher voxelMesher, Chunk chunk) : base(logicServer)
    {
      this.meshReciever = meshReciever;
      this.voxelMesher = voxelMesher;
      this.chunk = chunk;
    }


    /// <inheritdoc/>
    protected override void ExecuteImpl()
    {
      var mesh = this.voxelMesher.GenerateMesh(this.chunk);
      var task = new ApplyChunkMeshTask(LogicServer, mesh, this.chunk.Position, this.meshReciever);
      LogicServer.QueueRenderingTask(task);
    }
  }


  /// <summary>
  /// This render task is used to forward a mesh data object to a chunk mesh
  /// reciever instance in order for changes to that chunk mesh to be reflected
  /// in the visual aspect of the game engine.
  /// </summary>
  public class ApplyChunkMeshTask : RenderingTask
  {
    private readonly MeshData meshData;
    private readonly BlockPos chunkPos;
    private readonly IChunkMeshReciever meshReciever;


    /// <summary>
    /// Creates a new ApplyChunkMeshTask instance.
    /// </summary>
    /// <param name="logicServer">The logic server this task is being executed on.</param>
    /// <param name="meshData">The newly generated chunk mesh data.</param>
    /// <param name="chunkPos">The position of the chunk in block coordinates.</param>
    /// <param name="meshReciever">The reciever object to handle updating the Unity mesh object.</param>
    public ApplyChunkMeshTask(LogicServer logicServer, MeshData meshData, BlockPos chunkPos, IChunkMeshReciever meshReciever) : base(logicServer)
    {
      this.meshData = meshData;
      this.chunkPos = chunkPos;
      this.meshReciever = meshReciever;
    }


    /// <inheritdoc/>
    protected override void ExecuteImpl()
    {
      this.meshReciever.UpdateMesh(this.chunkPos, this.meshData);
    }
  }
}
