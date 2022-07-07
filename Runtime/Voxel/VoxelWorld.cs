using UnityEngine;
using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  public class VoxelWorld : MonoBehaviour
  {
    [SerializeField]
    private Material material;

    [SerializeField]
    private Mesh cubeMesh;

    private Dictionary<Chunk, GameObject> chunkObjects = new Dictionary<Chunk, GameObject>();

    public World World { get; private set; }

    protected void Awake()
    {
      World = new World();
      World.onChunkLoaded += OnChunkLoaded;
      World.onBlockUpdated += OnBlockUpdated;
    }

    protected void Start()
    {
      var p1 = new BlockPos(-3, -3, -3);
      var p2 = new BlockPos(3, 3, 3);
      foreach (var pos in CuboidIterator.FromTwoPoints(p1, p2))
      {
        var block = World.GetBlock(pos, true);
        block.SetBlockType(BlockRegistry.STONE_BLOCK);
      }
    }

    private void OnChunkLoaded(ChunkLoadedEvent e)
    {
      var chunk = e.GetChunk();

      var chunkObject = new GameObject($"Chunk {chunk.GetChunkPosition() >> 4}");
      chunkObject.transform.SetParent(transform, false);
      chunkObject.transform.localPosition = chunk.GetChunkPosition().AsVector3;

      chunkObject.AddComponent<MeshFilter>();
      chunkObject.AddComponent<MeshRenderer>().sharedMaterial = this.material;

      this.chunkObjects[chunk] = chunkObject;
    }

    private void OnBlockUpdated(BlockUpdatedEvent e)
    {
      var chunk = e.GetBlock().GetChunk();
      var mesher = new SimpleMesher(this.cubeMesh);
      var mesh = mesher.GenerateMesh(chunk);

      var chunkObject = this.chunkObjects[chunk];
      var meshFilter = chunkObject.GetComponent<MeshFilter>();
      meshFilter.sharedMesh = mesh;
    }
  }
}
