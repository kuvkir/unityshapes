using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CurvePath : Curve
{
  public List<Curve> curves;
  public bool autoClose;

  private float[] cacheLengths;

  public CurvePath()
  {
    curves = new List<Curve>();
    autoClose = false;
  }

  public void Add(Curve curve)
  {
    curves.Add(curve);
  }

  public void ClosePath()
  {
    // Add a line curve if start and end of lines are not connected
    Vector2 startPoint = curves[0].GetPoint(0);
    Vector2 endPoint = curves[curves.Count - 1].GetPoint(1);

    if (!startPoint.Equals(endPoint))
    {
      curves.Add(new LineCurve(endPoint, startPoint));
    }
  }

  // To get accurate point with reference to entire path distance at time t,
  // following has to be done:
  // 1. Length of each sub path have to be known
  // 2. Locate and identify type of curve
  // 3. Get t for the curve
  // 4. Return curve.GetPointAt(t')

  public override Vector2 GetPoint(float t, Vector2 optionalTarget = default)
  {
    float d = t * GetLength();
    float[] curveLengths = GetCurveLengths();
    int i = 0;

    // To think about boundaries points.

    while (i < curveLengths.Length)
    {
      if (curveLengths[i] >= d)
      {
        float diff = curveLengths[i] - d;
        Curve curve = curves[i];

        float segmentLength = curve.GetLength();
        float u = segmentLength == 0 ? 0 : 1 - diff / segmentLength;

        return curve.GetPointAt(u, optionalTarget);
      }

      i++;
    }

    return Vector2.zero;

    // loop where sum != 0, sum > d , sum+1 <d
  }

  // We cannot use the default GetLength() with GetCurveLengths() because in
  // Curve, GetLength() depends on GetPoint() but in CurvePath GetPoint() depends on GetLength

  public override float GetLength()
  {
    float[] lens = GetCurveLengths();
    return lens[lens.Length - 1];
  }

  // cacheLengths must be recalculated.
  public override void UpdateArcLengths()
  {
    needsUpdate = true;
    cacheLengths = null;
    GetCurveLengths();
  }

  // Compute lengths and cache them
  // We cannot overwrite GetLengths() because UtoT mapping uses it.

  public float[] GetCurveLengths()
  {
    // We use cache values if curves and cache array are same length

    if (cacheLengths != null && cacheLengths.Length == curves.Count)
    {
      return cacheLengths;
    }

    // Get length of sub-curve
    // Push sums into cached array

    float[] lengths = new float[curves.Count];
    float sums = 0;

    for (int i = 0; i < curves.Count; i++)
    {
      sums += curves[i].GetLength();
      lengths[i] = sums;
    }

    cacheLengths = lengths;

    return lengths;
  }

  public override Vector2[] GetSpacedPoints(int divisions = 40)
  {
    var points = new List<Vector2>();

    for (int i = 0; i <= divisions; i++)
    {
      points.Add(GetPoint(i / (float)divisions));
    }

    if (autoClose && points.Count > 1 && !points[points.Count - 1].Equals(points[0]))
    {
      points.Add(points[0]);
    }

    return points.ToArray();
  }

  public override Vector2[] GetPoints(int divisions = 12)
  {
    List<Vector2> points = new List<Vector2>();
    Vector2? last = null;

    foreach (var curve in curves)
    {
      int resolution =
        curve is EllipseCurve
          ? divisions * 2
          : curve is LineCurve
            //  || curve is LineCurve3
            ? 1
            // : curve is SplineCurve spline
            //   ? divisions * spline.points.Count
            : divisions;

      Vector2[] pts = curve.GetPoints(resolution);

      foreach (var point in pts)
      {
        if (last.HasValue && last.Value.Equals(point))
        {
          continue; // ensures no consecutive points are duplicates
        }

        points.Add(point);
        last = point;
      }
    }

    if (autoClose && points.Count > 1 && !points[points.Count - 1].Equals(points[0]))
    {
      points.Add(points[0]);
    }

    return points.ToArray();
  }
}
