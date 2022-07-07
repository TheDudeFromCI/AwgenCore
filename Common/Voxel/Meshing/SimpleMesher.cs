using UnityEngine;
using System.Collections.Generic;

namespace AwgenCore.Voxel
{
  /// <summary>
  /// A standard implementation of a voxel mesher that merely generates a quad
  /// for each visible face of a block.
  /// </summary>
  public class SimpleMesher : IVoxelMesher
  {
    private readonly List<int> cubeTriangles = new List<int>();
    private readonly List<Vector3> cubeVertices = new List<Vector3>();
    private readonly List<Vector3> cubeNormals = new List<Vector3>();
    private readonly List<Vector2> cubeUVs = new List<Vector2>();


    /// <summary>
    /// Creates a new SimpleMesher instance.
    /// </summary>
    /// <param name="cubeModel">The mesh model to use for all cubes.</param>
    public SimpleMesher(Mesh cubeModel)
    {
      cubeModel.GetTriangles(this.cubeTriangles, 0);
      cubeModel.GetVertices(this.cubeVertices);
      cubeModel.GetNormals(this.cubeNormals);
      cubeModel.GetUVs(0, this.cubeUVs);
    }


    /// <inheritdoc/>
    public Mesh GenerateMesh(Chunk chunk)
    {
      var vertices = new List<Vector3>();
      var normals = new List<Vector3>();
      var uvs = new List<Vector2>();
      var triangles = new List<int>();

      foreach (var blockPos in CuboidIterator.OverChunk())
      {
        var block = chunk.GetBlock(blockPos);
        var type = block.GetBlockType();

        foreach (var direction in Direction.All)
        {
          if (!type.OccludesSurface(direction)) continue;

          var relativePos = blockPos.Offset(direction, 1);
          var neighbor = GetBlock(chunk, relativePos)?.GetBlockType() ?? BlockRegistry.VOID_BLOCK;

          if (neighbor.OccludesSurface(direction.Opposite)) continue;

          var vertexCount = vertices.Count;
          foreach (var index in this.cubeTriangles)
            triangles.Add(index + vertexCount);

          foreach (var vertex in this.cubeVertices)
            vertices.Add(vertex + blockPos.AsVector3);

          normals.AddRange(this.cubeNormals);
          uvs.AddRange(this.cubeUVs);
        }
      }

      var mesh = new Mesh();
      mesh.SetVertices(vertices);
      mesh.SetNormals(normals);
      mesh.SetUVs(0, uvs);
      mesh.SetTriangles(triangles, 0);
      mesh.RecalculateBounds();
      mesh.RecalculateTangents();
      mesh.Optimize();
      return mesh;
    }


    /// <summary>
    /// A safe version of block retreval that will check if a block is inside of
    /// the chunk first before returning it. If the block is outside of the
    /// chunk, then it grabs the block from the chunk's world object instead.
    /// </summary>
    /// <param name="chunk">The chunk to attempt to read from in chunk local coordinates.</param>
    /// <param name="pos">The position of the block.</param>
    /// <returns></returns>
    private Block GetBlock(Chunk chunk, BlockPos pos)
    {
      if ((pos & 15) == pos) return chunk.GetBlock(pos);
      pos += chunk.GetChunkPosition();
      return chunk.GetWorld().GetBlock(pos, false);
    }
  }
}
