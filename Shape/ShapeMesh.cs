using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ShapeMesh
{
  public static Mesh build(Shape shape, int curveSegments = 12) =>
    build(new Shape[] { shape }, curveSegments);

  public static Mesh build(Shape[] shapes, int curveSegments = 12)
  {
    Mesh mesh = new Mesh();
    // int curveSegments;

    // Buffers
    var indices = new List<int>();
    var vertices = new List<Vector3>();
    var normals = new List<Vector3>();
    var uvs = new List<Vector2>();

    // Helper variables
    int groupCount = 0;

    for (int i = 0; i < shapes.Length; i++)
    {
      int indexOffset = vertices.Count;
      Shape shape = shapes[i];
      List<Vector2> shapeVertices = shape.GetPoints(curveSegments).ToList();
      List<List<Vector2>> shapeHoles = shape.GetPointsHoles(curveSegments);

      if (!ShapeUtils.IsClockWise(shapeVertices))
      {
        shapeVertices.Reverse();
      }

      foreach (var shapeHole in shapeHoles)
      {
        if (ShapeUtils.IsClockWise(shapeHole))
          shapeHole.Reverse();
      }

      var faces = ShapeUtils.TriangulateShape(shapeVertices, shapeHoles);
      // Join vertices of inner and outer paths to a single array
      for (int j = 0; j < shapeHoles.Count; j++)
      {
        var shapeHole = shapeHoles[j];
        shapeVertices.AddRange(shapeHole);
      }

      // Vertices, normals, uvs
      foreach (var vertex in shapeVertices)
      {
        vertices.Add(new Vector3(vertex.x, vertex.y, 0));
        normals.Add(new Vector3(0, 0, 1));
        uvs.Add(vertex); // world uvs
      }

      // Indices
      for (int j = 0; j < faces.Count; j++)
      {
        var face = faces[j];

        int a = face[0] + indexOffset;
        int b = face[1] + indexOffset;
        int c = face[2] + indexOffset;

        indices.AddRange(new[] { a, b, c });
        groupCount += 3;
      }
      // mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, i);
    }

    // Build geometry
    // mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
    // mesh.SetTriangles();
    mesh.SetVertices(vertices);
    // mesh.SetTriangles(indices.ToArray());
    mesh.triangles = indices.ToArray();
    // mesh.vertices = vertices.ToArray();
    // mesh.triangles = indices.ToArray();
    mesh.SetNormals(normals);
    mesh.SetUVs(0, uvs);
    mesh.RecalculateBounds();
    return mesh;
  }
}
