using UnityEngine;
using System.Collections.Generic;

public static class ShapeUtils
{
  // calculate area of the contour polygon
  public static float Area(List<Vector2> contour)
  {
    int n = contour.Count;
    float a = 0.0f;

    for (int p = n - 1, q = 0; q < n; p = q++)
    {
      a += contour[p].x * contour[q].y - contour[q].x * contour[p].y;
    }

    return a * 0.5f;
  }

  public static bool IsClockWise(List<Vector2> pts)
  {
    return Area(pts) < 0;
  }

  public static List<int[]> TriangulateShape(List<Vector2> contour, List<List<Vector2>> holes)
  {
    List<float> vertices = new List<float>(); // flat array of vertices like [ x0,y0, x1,y1, x2,y2, ... ]
    List<int> holeIndices = new List<int>(); // array of hole indices
    List<int[]> faces = new List<int[]>(); // final array of vertex indices like [ [ a,b,d ], [ b,c,d ] ]

    RemoveDupEndPts(contour);
    AddContour(vertices, contour);

    int holeIndex = contour.Count;

    foreach (var hole in holes)
    {
      RemoveDupEndPts(hole);
      holeIndices.Add(holeIndex);
      holeIndex += hole.Count;
      AddContour(vertices, hole);
    }

    int[] triangles = Earcut.Tessellate(vertices.ToArray(), holeIndices.ToArray());

    for (int i = 0; i < triangles.Length; i += 3)
    {
      faces.Add(new int[] { triangles[i], triangles[i + 1], triangles[i + 2] });
    }

    return faces;
  }

  private static void RemoveDupEndPts(List<Vector2> points)
  {
    int l = points.Count;

    if (l > 2 && points[l - 1].Equals(points[0]))
    {
      points.RemoveAt(l - 1);
    }
  }

  private static void AddContour(List<float> vertices, List<Vector2> contour)
  {
    foreach (var point in contour)
    {
      vertices.Add(point.x);
      vertices.Add(point.y);
    }
  }
}
