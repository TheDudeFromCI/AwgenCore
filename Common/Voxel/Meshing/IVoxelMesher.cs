namespace AwgenCore.Voxel
{
  /// <summary>
  /// A handler that can be used to convert a voxel chunk into a usable mesh
  /// object.
  /// </summary>
  public interface IVoxelMesher
  {
    /// <summary>
    /// Create a mesh object based on the given chunk.
    /// </summary>
    /// <param name="chunk">The chunk to process.</param>
    /// <returns>A new mesh object.</returns>
    MeshData GenerateMesh(Chunk chunk);
  }
}
