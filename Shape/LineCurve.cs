using UnityEngine;

public class LineCurve : Curve
{
  public Vector2 v1;
  public Vector2 v2;

  public LineCurve(Vector2 v1 = default, Vector2 v2 = default)
  {
    this.v1 = v1;
    this.v2 = v2;
  }

  public override Vector2 GetPoint(float t, Vector2 optionalTarget = default)
  {
    var point = optionalTarget;

    if (t == 1)
    {
      point = v2;
    }
    else
    {
      point = v2 - v1;
      point *= t;
      point += v1;
    }

    return point;
  }

  public override Vector2 GetPointAt(float u, Vector2 optionalTarget = default)
  {
    return GetPoint(u, optionalTarget);
  }

  public override Vector2 GetTangent(float t, Vector2 optionalTarget = default)
  {
    return (v2 - v1).normalized;
  }

  public override Vector2 GetTangentAt(float u, Vector2 optionalTarget = default)
  {
    return GetTangent(u, optionalTarget);
  }
}
