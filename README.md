# Unity Shapes API (Ported from Three.js)

This is a port of the
[Three.js Shapes API](https://threejs.org/docs/index.html?q=shape#api/en/extras/core/Shape)
to Unity. It provides a way to create procedural 2D shapes, add holes to them,
and triangulate them for rendering in Unity.

![smileyface](https://github.com/kittygiant/unityshapes/blob/main/smileyface.png?raw=true)
![star](https://github.com/kittygiant/unityshapes/blob/main/star.png?raw=true)
![rectangle](https://github.com/kittygiant/unityshapes/blob/main/rectangle.png?raw=true)

## Usage

To use this library in your Unity project, simply download or clone this
repository and add the Shapes folder to your Unity project's Assets directory.

You can then use the `Shape` class to create a shape.

```csharp
using UnityEngine;

// Create a shape
Shape smileyFace = new Shape();
smileyFace.MoveTo(0, 0);
smileyFace.Absellipse(0, 0, 2f, 2f, 0, Mathf.PI * 2);

// Create the left eye shape
var leftEyeShape = new Shape();
leftEyeShape.MoveTo(-0.5f, 0);
leftEyeShape.Absarc(-0.5f, 0.5f, 0.25f, 0, Mathf.PI * 2);

// Create the right eye shape
var rightEyeShape = new Shape();
rightEyeShape.MoveTo(0.5f, 0);
rightEyeShape.Absarc(0.5f, 0.5f, 0.25f, 0, Mathf.PI * 2);

// Create the mouth shape
var mouthShape = new Shape();
mouthShape.MoveTo(-1, -0);
mouthShape.Absellipse(0, -0.5f, 1, 1, 0, Mathf.PI, true);

smileyFace.holes.Add(leftEyeShape);
smileyFace.holes.Add(rightEyeShape);
smileyFace.holes.Add(mouthShape);
```

To render a shape, you can create a mesh using `ShapeMesh` as follows

```csharp
var shapeGO = new GameObject("Shape");
var meshFilter = shapeGO.AddComponent<MeshFilter>();
meshFilter.mesh = ShapeMesh.build(shape, 24);
var meshRenderer = shapeGO.AddComponent<MeshRenderer>();
meshRenderer.material = new Material(Shader.Find("Standard"));
meshRenderer.material.color = Color.yellow;
```

## API

The Shape class provides the following methods for creating and manipulating
shapes:

- `MoveTo(x, y)` - Move the pen to the specified position without drawing.
- `LineTo(x, y)` - Draw a line from the pen's current position to the specified
  position.
- `BezierCurveTo(cp1x, cp1y, cp2x, cp2y, x, y)` - Draw a cubic Bezier curve from
  the pen's current position to the specified position using the specified
  control points.
- `QuadraticCurveTo(cpx, cpy, x, y)` - Draw a quadratic Bezier curve from the
  pen's current position to the specified position using the specified control
  point.
- `Absarc(x, y, radius, startAngle, endAngle, anticlockwise)` - Draw a circular
  arc with the specified center, radius, start angle, end angle, and direction
  (clockwise or counterclockwise).
- `Ellipse(x, y, xRadius, yRadius, rotation, startAngle, endAngle, anticlockwise)` -
  Draw an elliptical arc with the specified center, radii, rotation, start
  angle, end angle, and direction (clockwise or counterclockwise).
- `Absellipse(x, y, xRadius, yRadius, rotation, startAngle, endAngle, anticlockwise)` -
  Draw an elliptical arc with the specified center, radii, rotation, start
  angle, end angle, and direction (clockwise or counterclockwise) using absolute
  values.
- `ClosePath()` - Close the current path by drawing a line to the first point in
  the path.
- `GetPoints(divisions)` - Get a list of points on the shape's path.

Alternatively, you can create a Shape and manually provide the list of vertices
as follows

```csharp
List<Vector2> points = new List<Vector2>();
for (var i = 0; i < 5 * 2; i++)
{
  var l = i % 2 == 1 ? 2f : 0.75f;
  var a = i * Mathf.PI / 5;
  points.Add(new Vector2(Mathf.Sin(a) * l, Mathf.Cos(a) * l));
}
Shape shape = new Shape(points);
```

## License

This library is licensed under the MIT License.

---

_Note: This port was made possible by the tireless efforts of ChatGPT, who was
instrumental in helping with the porting process._
