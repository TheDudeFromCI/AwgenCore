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
    private readonly MeshData cubeMesh;


    /// <summary>
    /// Creates a new SimpleMesher instance.
    /// </summary>
    /// <param name="cubeMesh">The mesh data to use for all cubes.</param>
    public SimpleMesher(MeshData cubeMesh)
    {
      this.cubeMesh = cubeMesh;
    }


    /// <inheritdoc/>
    public MeshData GenerateMesh(Chunk chunk)
    {
      var mesh = new MeshData();

      foreach (var blockPos in CuboidIterator.OverChunk())
      {
        var block = chunk[blockPos];
        var type = block.BlockType;

        foreach (var direction in Direction.All)
        {
          if (!type.OccludesSurface(direction)) continue;

          var relativePos = blockPos.Offset(direction, 1);
          var neighbor = GetBlock(chunk, relativePos)?.BlockType ?? BlockRegistry.VOID_BLOCK;
          if (neighbor.OccludesSurface(direction.Opposite)) continue;

          mesh.AppendMesh(this.cubeMesh, blockPos.AsVector3);
        }
      }

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
      if ((pos & 15) == pos) return chunk[pos];
      pos += chunk.Position;
      return chunk.World.GetBlock(pos, false);
    }
  }
}
