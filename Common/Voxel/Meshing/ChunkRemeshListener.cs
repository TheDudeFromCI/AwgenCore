using System;

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
      var chunk = ev.GetChunk();
      RegenerateChunk(chunk);

      RegenerateChunk(chunk.World.GetChunk(chunk.Position.Offset(Direction.North, 16), false));
      RegenerateChunk(chunk.World.GetChunk(chunk.Position.Offset(Direction.East, 16), false));
      RegenerateChunk(chunk.World.GetChunk(chunk.Position.Offset(Direction.South, 16), false));
      RegenerateChunk(chunk.World.GetChunk(chunk.Position.Offset(Direction.West, 16), false));
      RegenerateChunk(chunk.World.GetChunk(chunk.Position.Offset(Direction.Up, 16), false));
      RegenerateChunk(chunk.World.GetChunk(chunk.Position.Offset(Direction.Down, 16), false));
    }


    /// <summary>
    /// An event that is called whenever a block is updated in the world.
    /// </summary>
    /// <param name="ev">The event.</param>
    private void OnBlockUpdated(BlockUpdatedEvent ev)
    {
      var block = ev.GetBlock();
      RegenerateChunk(block.Chunk);

      Func<BlockPos, Chunk> GetChunk = delegate (BlockPos pos)
      {
        if (pos >> 4 == block.Position >> 4) return null;
        return block.World.GetChunk(pos, false);
      };

      RegenerateChunk(GetChunk(block.Position + Direction.North));
      RegenerateChunk(GetChunk(block.Position + Direction.East));
      RegenerateChunk(GetChunk(block.Position + Direction.South));
      RegenerateChunk(GetChunk(block.Position + Direction.West));
      RegenerateChunk(GetChunk(block.Position + Direction.Up));
      RegenerateChunk(GetChunk(block.Position + Direction.Down));
    }


    /// <summary>
    /// Creates and queues a remeshing worker task for the given chunk.
    /// </summary>
    /// <param name="chunk">The chunk to generate a mesh for.</param>
    private void RegenerateChunk(Chunk chunk)
    {
      if (chunk == null) return;
      var task = new ChunkRemeshTask(this.logicServer, this.meshReciever, this.voxelMesher, chunk);
      this.logicServer.QueueWorkerTask(task);
    }
  }
}
