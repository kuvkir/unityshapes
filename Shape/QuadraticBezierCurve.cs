using UnityEngine;

public class QuadraticBezierCurve : Curve
{
  public Vector2 v0;
  public Vector2 v1;
  public Vector2 v2;

  public QuadraticBezierCurve(Vector2 v0 = default, Vector2 v1 = default, Vector2 v2 = default)
  {
    this.v0 = v0;
    this.v1 = v1;
    this.v2 = v2;
  }

  public override Vector2 GetPoint(float t, Vector2 optionalTarget = default)
  {
    Vector2 point = optionalTarget != default ? optionalTarget : new Vector2();

    point.Set(
      Interpolation.QuadraticBezier(t, v0.x, v1.x, v2.x),
      Interpolation.QuadraticBezier(t, v0.y, v1.y, v2.y)
    );

    return point;
  }
}
