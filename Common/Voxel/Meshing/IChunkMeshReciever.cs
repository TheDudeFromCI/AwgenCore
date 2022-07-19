namespace AwgenCore.Voxel
{
  /// <summary>
  /// For objects that store chunk meshes, this interface is used to recieve
  /// updates that occur to those chunks in order to update their meshes as the
  /// chunks are updated.
  /// </summary>
  public interface IChunkMeshReciever
  {
    /// <summary>
    /// This method is called by the ChunkRemeshTask whenever a chunk's mesh
    /// has finished being regenerated. This method will only be called on the
    /// render thread. This method may be called on either existing chunks or on
    /// newly loaded chunks.
    /// </summary>
    /// <param name="chunkPos">The position of the chunk in block coordinates.</param>
    /// <param name="meshData">The mesh data to apply to the chunk.</param>
    void UpdateMesh(BlockPos chunkPos, MeshData meshData);
  }
}
