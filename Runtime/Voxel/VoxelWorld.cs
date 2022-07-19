using UnityEngine;
using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  public class VoxelWorld : MonoBehaviour, IChunkMeshReciever, IWorldGenerator
  {
    [SerializeField]
    private Material material;

    private Dictionary<BlockPos, Mesh> chunkObjects = new Dictionary<BlockPos, Mesh>();

    public World World { get; private set; }


    protected void Awake()
    {
      World = new World(this);
    }


    protected void Start()
    {
      var registry = AwgenCore.Instance.GetOrCreateRegistry<BlockType>();
      var voidBlock = registry["awgen:void"];
      var simpleMesher = new SimpleMesher(voidBlock);
      new ChunkRemeshListener(AwgenCore.Instance.LogicServer, World, this, simpleMesher);

      var anchor = new ChunkLoadingAnchor();
      anchor.ViewDistance = 5;
      var chunkLoadingTask = new ChunkLoadingTask(AwgenCore.Instance.LogicServer, World, anchor);
      AwgenCore.Instance.LogicServer.AddLogicUpdate(chunkLoadingTask);
    }


    private Mesh CreateChunk(BlockPos pos)
    {
      var chunkObject = new GameObject($"Chunk {pos >> 4}");
      chunkObject.transform.SetParent(this.transform, false);
      chunkObject.transform.localPosition = pos.AsVector3;

      Mesh mesh;
      chunkObject.AddComponent<MeshFilter>().sharedMesh = mesh = new Mesh();
      chunkObject.AddComponent<MeshRenderer>().sharedMaterial = this.material;

      this.chunkObjects[pos] = mesh;
      return mesh;
    }


    public void UpdateMesh(BlockPos chunkPos, MeshData meshData)
    {
      Mesh chunkMesh;
      if (this.chunkObjects.ContainsKey(chunkPos)) chunkMesh = this.chunkObjects[chunkPos];
      else chunkMesh = CreateChunk(chunkPos);

      meshData.UploadToUnity(chunkMesh);
    }


    public QualifiedName<BlockType>[] GenerateChunk(BlockPos chunkPos)
    {
      var registry = AwgenCore.Instance.GetOrCreateRegistry<BlockType>();
      var voidBlock = new QualifiedName<BlockType>(registry, "awgen:void");
      var stoneBlock = new QualifiedName<BlockType>(registry, "awgen:stone");

      var blocks = new QualifiedName<BlockType>[Chunk.BLOCK_COUNT];
      for (var i = 0; i < blocks.Length; i++) blocks[i] = chunkPos.y >= 0 ? voidBlock : stoneBlock;
      return blocks;
    }
  }
}
