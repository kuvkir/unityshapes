using UnityEngine;

public class EllipseCurve : Curve
{
  public float aX;
  public float aY;
  public float xRadius;
  public float yRadius;
  public float aStartAngle;
  public float aEndAngle;
  public bool aClockwise;
  public float aRotation;

  public EllipseCurve(
    float aX = 0,
    float aY = 0,
    float xRadius = 1,
    float yRadius = 1,
    float aStartAngle = 0,
    float aEndAngle = Mathf.PI * 2,
    bool aClockwise = false,
    float aRotation = 0
  )
  {
    this.aX = aX;
    this.aY = aY;
    this.xRadius = xRadius;
    this.yRadius = yRadius;
    this.aStartAngle = aStartAngle;
    this.aEndAngle = aEndAngle;
    this.aClockwise = aClockwise;
    this.aRotation = aRotation;
  }

  public override Vector2 GetPoint(float t, Vector2 optionalTarget = default)
  {
    Vector2 point = optionalTarget != default ? optionalTarget : new Vector2();

    const float twoPi = Mathf.PI * 2;
    float deltaAngle = this.aEndAngle - this.aStartAngle;
    bool samePoints = Mathf.Abs(deltaAngle) < Mathf.Epsilon;

    // ensures that deltaAngle is 0 .. 2 PI
    while (deltaAngle < 0)
      deltaAngle += twoPi;
    while (deltaAngle > twoPi)
      deltaAngle -= twoPi;

    if (deltaAngle < Mathf.Epsilon)
    {
      if (samePoints)
      {
        deltaAngle = 0;
      }
      else
      {
        deltaAngle = twoPi;
      }
    }

    if (this.aClockwise && !samePoints)
    {
      if (deltaAngle == twoPi)
      {
        deltaAngle = -twoPi;
      }
      else
      {
        deltaAngle = deltaAngle - twoPi;
      }
    }

    float angle = this.aStartAngle + t * deltaAngle;
    float x = this.aX + this.xRadius * Mathf.Cos(angle);
    float y = this.aY + this.yRadius * Mathf.Sin(angle);

    if (this.aRotation != 0)
    {
      float cos = Mathf.Cos(this.aRotation);
      float sin = Mathf.Sin(this.aRotation);

      float tx = x - this.aX;
      float ty = y - this.aY;

      // Rotate the point about the center of the ellipse.
      x = tx * cos - ty * sin + this.aX;
      y = tx * sin + ty * cos + this.aY;
    }

    point.Set(x, y);
    return point;
  }
}
