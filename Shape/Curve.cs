using UnityEngine;

#nullable disable
public class Curve
{
  public string type;
  public int arcLengthDivisions;
  public bool needsUpdate;
  public float[] cacheArcLengths;

  public Curve()
  {
    arcLengthDivisions = 200;
  }

  public virtual Vector2 GetPoint(float t, Vector2 optionalTarget = default)
  {
    Debug.LogWarning("Curve.GetPoint() not implemented.");
    return Vector2.zero;
  }

  public virtual Vector2 GetPointAt(float u, Vector2 optionalTarget = default)
  {
    float t = GetUtoTmapping(u);
    return GetPoint(t, optionalTarget);
  }

  public virtual Vector2[] GetPoints(int divisions = 5)
  {
    Vector2[] points = new Vector2[divisions + 1];

    for (int d = 0; d <= divisions; d++)
    {
      points[d] = GetPoint((float)d / divisions);
    }

    return points;
  }

  public virtual Vector2[] GetSpacedPoints(int divisions = 5)
  {
    Vector2[] points = new Vector2[divisions + 1];

    for (int d = 0; d <= divisions; d++)
    {
      points[d] = GetPointAt((float)d / divisions);
    }

    return points;
  }

  public virtual float GetLength()
  {
    float[] lengths = GetLengths();
    return lengths[lengths.Length - 1];
  }

  public float[] GetLengths(int divisions = -1)
  {
    if (cacheArcLengths != null && cacheArcLengths.Length == divisions + 1 && !needsUpdate)
    {
      return cacheArcLengths;
    }

    needsUpdate = false;
    divisions = divisions < 0 ? arcLengthDivisions : divisions;

    float[] cache = new float[divisions + 1];
    Vector2 current,
      last = GetPoint(0);
    float sum = 0;
    cache[0] = 0;

    for (int p = 1; p <= divisions; p++)
    {
      current = GetPoint((float)p / divisions);
      sum += Vector2.Distance(current, last);
      cache[p] = sum;
      last = current;
    }

    cacheArcLengths = cache;
    return cache;
  }

  public virtual void UpdateArcLengths()
  {
    needsUpdate = true;
    GetLengths();
  }

  public float GetUtoTmapping(float u, float distance = -1)
  {
    float[] arcLengths = GetLengths();

    int i = 0;
    int il = arcLengths.Length;

    float targetArcLength;

    if (distance >= 0)
    {
      targetArcLength = distance;
    }
    else
    {
      targetArcLength = u * arcLengths[il - 1];
    }

    int low = 0;
    int high = il - 1;
    int comparison;

    while (low <= high)
    {
      i = (int)Mathf.Floor(low + (high - low) / 2);
      comparison = (int)(arcLengths[i] - targetArcLength);

      if (comparison < 0)
      {
        low = i + 1;
      }
      else if (comparison > 0)
      {
        high = i - 1;
      }
      else
      {
        high = i;
        break;
      }
    }

    i = high;

    if (arcLengths[i] == targetArcLength)
    {
      return i / (float)(il - 1);
    }

    float lengthBefore = arcLengths[i];

    float lengthAfter = arcLengths[i + 1];
    float segmentLength = lengthAfter - lengthBefore;
    float segmentFraction = (targetArcLength - lengthBefore) / segmentLength;
    float t = (i + segmentFraction) / (float)(il - 1);

    return t;
  }

  public virtual Vector2 GetTangent(float t, Vector2 optionalTarget = default)
  {
    float delta = 0.0001f;
    float t1 = t - delta;
    float t2 = t + delta;

    if (t1 < 0)
    {
      t1 = 0;
    }

    if (t2 > 1)
    {
      t2 = 1;
    }

    Vector2 pt1 = GetPoint(t1);
    Vector2 pt2 = GetPoint(t2);

    Vector2 tangent = optionalTarget != default ? optionalTarget : new Vector2();
    tangent = pt2 - pt1;
    tangent.Normalize();

    return tangent;
  }

  public virtual Vector2 GetTangentAt(float u, Vector2 optionalTarget = default)
  {
    float t = GetUtoTmapping(u);
    return GetTangent(t, optionalTarget);
  }
}
