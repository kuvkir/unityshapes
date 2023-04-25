using System.Collections.Generic;
using UnityEngine;

public class Path : CurvePath
{
  public Vector2 currentPoint = Vector2.zero;

  public Path(List<Vector2> points)
  {
    if (points != null && points.Count > 0)
    {
      SetFromPoints(points);
    }
  }

  public void SetFromPoints(List<Vector2> points)
  {
    if (points.Count == 0)
      return;

    MoveTo(points[0].x, points[0].y);

    for (int i = 1; i < points.Count; i++)
    {
      LineTo(points[i].x, points[i].y);
    }
  }

  public Path MoveTo(float x, float y)
  {
    currentPoint.Set(x, y);

    return this;
  }

  public Path LineTo(float x, float y)
  {
    var curve = new LineCurve(currentPoint, new Vector2(x, y));
    curves.Add(curve);

    currentPoint.Set(x, y);

    return this;
  }

  public Path QuadraticCurveTo(float aCPx, float aCPy, float aX, float aY)
  {
    var curve = new QuadraticBezierCurve(
      new Vector2(currentPoint.x, currentPoint.y),
      new Vector2(aCPx, aCPy),
      new Vector2(aX, aY)
    );
    curves.Add(curve);
    currentPoint.Set(aX, aY);
    return this;
  }

  public Path Arc(
    float aX,
    float aY,
    float aRadius,
    float aStartAngle,
    float aEndAngle,
    bool aClockwise = false
  )
  {
    var x0 = currentPoint.x;
    var y0 = currentPoint.y;

    Absarc(aX + x0, aY + y0, aRadius, aStartAngle, aEndAngle, aClockwise);

    return this;
  }

  public Path Absarc(
    float aX,
    float aY,
    float aRadius,
    float aStartAngle,
    float aEndAngle,
    bool aClockwise = false
  )
  {
    Absellipse(aX, aY, aRadius, aRadius, aStartAngle, aEndAngle, aClockwise, 0);

    return this;
  }

  public Path Ellipse(
    float aX,
    float aY,
    float xRadius,
    float yRadius,
    float aStartAngle,
    float aEndAngle,
    bool aClockwise = false,
    float aRotation = 0
  )
  {
    var x0 = currentPoint.x;
    var y0 = currentPoint.y;

    Absellipse(aX + x0, aY + y0, xRadius, yRadius, aStartAngle, aEndAngle, aClockwise, aRotation);

    return this;
  }

  public Path Absellipse(
    float aX,
    float aY,
    float xRadius,
    float yRadius,
    float aStartAngle,
    float aEndAngle,
    bool aClockwise = false,
    float aRotation = 0
  )
  {
    var curve = new EllipseCurve(
      aX,
      aY,
      xRadius,
      yRadius,
      aStartAngle,
      aEndAngle,
      aClockwise,
      aRotation
    );

    if (curves.Count > 0)
    {
      var firstPoint = curve.GetPoint(0);

      if (!firstPoint.Equals(currentPoint))
      {
        LineTo(firstPoint.x, firstPoint.y);
      }
    }

    curves.Add(curve);

    var lastPoint = curve.GetPoint(1);
    currentPoint = lastPoint;

    return this;
  }
}
