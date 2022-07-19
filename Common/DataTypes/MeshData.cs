using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace AwgenCore
{
  /// <summary>
  /// A collection of mesh data that can be created and modified in a
  /// thread-safe manner to be uploaded into a Unity mesh at a later time.
  /// </summary>
  public class MeshData
  {
    private const int MAX_VERTICES = 65536;


    /// <summary>
    /// Creates a new MeshData instance from an existing Unity mesh object.
    /// </summary>
    /// <param name="mesh">The Unity mesh to read from.</param>
    /// <returns>A new MeshData instance.</returns>
    public static MeshData CreateFromUnityMesh(Mesh mesh)
    {
      var data = new MeshData();
      mesh.GetVertices(data.Vertices);
      mesh.GetNormals(data.Normals);
      mesh.GetUVs(0, data.UVs);
      mesh.GetTriangles(data.Triangles, 0);
      return data;
    }


    /// <summary>
    /// A list of all vertex locations within this mesh.
    /// </summary>
    public readonly List<Vector3> Vertices = new List<Vector3>();


    /// <summary>
    /// A list of all normals within this mesh.
    /// </summary>
    public readonly List<Vector3> Normals = new List<Vector3>();


    /// <summary>
    /// A list of all UVs within this mesh.
    /// </summary>
    public readonly List<Vector2> UVs = new List<Vector2>();


    /// <summary>
    /// A list of all triangles within this mesh.
    /// </summary>
    public readonly List<int> Triangles = new List<int>();


    /// <summary>
    /// Uploads this mesh data into a Unity mesh. This method should only be
    /// called from within the render thread.
    /// </summary>
    /// <param name="mesh">The mesh to upload data to.</param>
    public void UploadToUnity(Mesh mesh)
    {
      mesh.Clear();
      mesh.indexFormat = Vertices.Count > MAX_VERTICES ? IndexFormat.UInt32 : IndexFormat.UInt16;
      mesh.SetVertices(Vertices);
      mesh.SetNormals(Normals);
      mesh.SetUVs(0, UVs);
      mesh.SetTriangles(Triangles, 0);
      mesh.RecalculateTangents();
      mesh.Optimize();
    }


    /// <summary>
    /// Appends another mesh object to the end of this mesh.
    /// </summary>
    /// <param name="mesh">The mesh data to append.</param>
    /// <param name="positionOffset">The position offset of the new mesh data.</param>
    public void AppendMesh(MeshData mesh, Vector3 positionOffset)
    {
      var vertexCount = Vertices.Count;
      foreach (var index in mesh.Triangles)
        Triangles.Add(index + vertexCount);

      foreach (var vertex in mesh.Vertices)
        Vertices.Add(vertex + positionOffset);

      Normals.AddRange(mesh.Normals);
      UVs.AddRange(mesh.UVs);
    }
  }
}
