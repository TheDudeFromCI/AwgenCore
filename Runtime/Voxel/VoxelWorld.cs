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
      var anchor = new ChunkLoadingAnchor();
      var chunkLoadingTask = new ChunkLoadingTask(AwgenCore.Instance.LogicServer, World, anchor);
      AwgenCore.Instance.LogicServer.AddLogicUpdate(chunkLoadingTask);
    }

    private void OnChunkLoaded(ChunkLoadedEvent e)
    {
      var chunk = e.GetChunk();

      var chunkObject = new GameObject($"Chunk {chunk.Position >> 4}");
      chunkObject.transform.SetParent(transform, false);
      chunkObject.transform.localPosition = chunk.Position.AsVector3;

      chunkObject.AddComponent<MeshFilter>().sharedMesh = new Mesh();
      chunkObject.AddComponent<MeshRenderer>().sharedMaterial = this.material;

      this.chunkObjects[chunk] = chunkObject;
    }

    private void OnBlockUpdated(BlockUpdatedEvent e)
    {
      var chunk = e.GetBlock().Chunk;
      var mesher = new SimpleMesher(MeshData.CreateFromUnityMesh(this.cubeMesh));
      var mesh = mesher.GenerateMesh(chunk);

      var chunkObject = this.chunkObjects[chunk];
      var meshFilter = chunkObject.GetComponent<MeshFilter>();
      mesh.UploadToUnity(meshFilter.sharedMesh);
    }
  }
}
