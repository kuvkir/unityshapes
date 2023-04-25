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

## License

This library is licensed under the MIT License.

---

_Note: This port was made possible by the tireless efforts of ChatGPT, who was
instrumental in helping with the porting process._
