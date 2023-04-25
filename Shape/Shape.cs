using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#nullable disable

public class Shape : Path
{
  public List<Path> holes = new List<Path>();

  public Shape(List<Vector2> points)
    : base(points) { }

  public Shape()
    : base(new List<Vector2>()) { }

  public List<List<Vector2>> GetPointsHoles(int divisions) =>
    holes.Select(h => h.GetPoints(divisions).ToList()).ToList();
}
