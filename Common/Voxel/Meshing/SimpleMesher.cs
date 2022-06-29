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

          GenerateQuadIndices(triangles, vertices.Count);
          GenerateQuadVertices(blockPos, direction, vertices);
          GenerateQuadNormals(direction, normals);
          GenerateQuadUVs(uvs);
        }
      }

      var mesh = new Mesh();
      mesh.SetVertices(vertices);
      mesh.SetNormals(normals);
      mesh.SetUVs(0, uvs);
      mesh.SetTriangles(triangles, 0);
      mesh.RecalculateBounds();
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


    /// <summary>
    /// Generates the next set of quad indices for a mesh and adds it to the
    /// index list.
    /// </summary>
    /// <param name="indices">The list of indices to append to.</param>
    /// <param name="vertexCount">The current number of vertices in the mesh.</param>
    private void GenerateQuadIndices(List<int> indices, int vertexCount)
    {
      indices.Add(vertexCount + 0);
      indices.Add(vertexCount + 1);
      indices.Add(vertexCount + 2);
      indices.Add(vertexCount + 0);
      indices.Add(vertexCount + 2);
      indices.Add(vertexCount + 3);
    }


    /// <summary>
    /// Generates the vertice locations for a quad and adds them to the vertex
    /// position list.
    /// </summary>
    /// <param name="pos">The local position of the block within the chunk.</param>
    /// <param name="direction">The direction, or surface, of the block to generate.</param>
    /// <param name="vertices">The vertex list to append to.</param>
    private void GenerateQuadVertices(BlockPos pos, Direction direction, List<Vector3> vertices)
    {
      var center = pos.AsVector3 + new Vector3(0.5f, 0.5f, 0.5f);
      var v0 = pos.AsVector3 + Vector3.Max(direction.AsVector3, Vector3.zero);
      var v1 = Quaternion.AngleAxis(90, direction.AsVector3) * (v0 - center) + center;
      var v2 = Quaternion.AngleAxis(90, direction.AsVector3) * (v1 - center) + center;
      var v3 = Quaternion.AngleAxis(90, direction.AsVector3) * (v2 - center) + center;

      vertices.Add(v0);
      vertices.Add(v1);
      vertices.Add(v2);
      vertices.Add(v3);
    }


    /// <summary>
    /// Generates the quad normals for a given direction and adds them to the
    /// normals list.
    /// </summary>
    /// <param name="direction">The direction of the quad.</param>
    /// <param name="normals">The normals list to append to.</param>
    private void GenerateQuadNormals(Direction direction, List<Vector3> normals)
    {
      var normal = direction.AsVector3;
      normals.Add(normal);
      normals.Add(normal);
      normals.Add(normal);
      normals.Add(normal);
    }


    /// <summary>
    /// Generates a set of UVs for the given quad face and adds them to the uvs
    /// list.
    /// </summary>
    /// <param name="uvs">The uvs list to append to.</param>
    private void GenerateQuadUVs(List<Vector2> uvs)
    {
      uvs.Add(new Vector2(1, 1));
      uvs.Add(new Vector2(1, 0));
      uvs.Add(new Vector2(0, 0));
      uvs.Add(new Vector2(0, 1));
    }
  }
}
