using UnityEngine;
using UnityEditor;
using System.Collections;

[RequireComponent (typeof (MeshCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class ADRPyramid : MonoBehaviour {

	private float _height = 2.0f;
	private float _radius = 1.0f;

	[HideInInspector]
	public int baseSides = 4;
	private int _baseSides = 4;

	private static Material _defaultDiffuse = null;

	void Update() {
		if (baseSides < 3) {
			baseSides = 3;
			_baseSides = 3;
			Rebuild();
		} else if (baseSides != _baseSides) {
			_baseSides = baseSides;
			Rebuild();
		}
	}

	public static void Create() {
		Create (Vector3.zero, Quaternion.identity);
	}

	public static void Create(Vector3 position, Quaternion rotation) {
		GameObject gameObject = new GameObject("Pyramid");
		ADRPyramid pyramid = gameObject.AddComponent<ADRPyramid>();
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh();
		pyramid.Rebuild();

		if (_defaultDiffuse == null) {
			GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
			_defaultDiffuse = primitive.GetComponent<MeshRenderer>().sharedMaterial;
			DestroyImmediate(primitive);
		}
		pyramid.GetComponent<Renderer> ().sharedMaterial = _defaultDiffuse;
	}

	private Vector3[] _CalculatePoints() {
		float halfHeight = _height / 2;
		Vector3[] points = new Vector3[baseSides + 1];
		points [0] = new Vector3 (0, halfHeight, 0);

		int i;
		float singleAngle = 360f / baseSides * Mathf.Deg2Rad;
		for (i=1; i<points.Length; i++) {
			float angle = singleAngle * (i-1);
			points[i] = new Vector3(Mathf.Cos (angle) * _radius, -halfHeight, Mathf.Sin (angle) * _radius);
		}

		return points;
	}

	private Vector2[] _CalculateUV() {
		Vector2[] uv = new Vector2[6 * baseSides - 6];
		float singleAngle = 360f / baseSides * Mathf.Deg2Rad;
		float d = _radius * Mathf.Cos (singleAngle / 2);
		float h = Mathf.Sqrt (d * d + _height * _height);
		float k = 1f / (2 * (h + d));
		float kr = _radius * k;
		Vector2 center = new Vector2 (0.5f, 0.5f);

		int i, j = 0;
		float angleSum = 0;
		for(i=0; i<baseSides; i++) {
			float angle1 = Mathf.PI - angleSum;
			float angle2 = Mathf.PI + (singleAngle / 2) - angleSum;
			float angle3 = Mathf.PI - (singleAngle / 2) - angleSum;

			uv[j] = center + (new Vector2(Mathf.Cos (angle1) * 0.5f, Mathf.Sin (angle1) * 0.5f));
			uv[j + 1] = center + (new Vector2(kr * Mathf.Cos (angle3), kr * Mathf.Sin (angle3)));
			uv[j + 2] = center + (new Vector2(kr * Mathf.Cos (angle2), kr * Mathf.Sin (angle2)));

			angleSum += singleAngle;
			j += 3;
		}

		for (i=0; i<baseSides - 2; i++) {
			uv[j] = uv[2];
			uv[j+1] = uv[1 + i * 3];
			uv[j+2] = uv[4 + i * 3];
			j += 3;
		}

		return uv;
	}

	public void Rebuild() {
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			Debug.LogError("[ADRPyramid] - MeshFilter not found!");
			return;
		}

		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.Clear();

		Vector3[] points = _CalculatePoints ();
		int numberOfVertices = 6 * baseSides - 6;
		Vector3[] vertices = new Vector3[numberOfVertices];
		int i, j = 0;
		for (i=0; i<baseSides; i++) {
			vertices[j] = points[0];
			vertices[j+2] = points[i + 1];
			vertices[j+1] = points[_nextBaseIndex(i + 1)];
			j+= 3;
		}
		for (i=0; i<baseSides-2; i++) {
			vertices[j] = points[1];
			vertices[j+2] = points[_nextBaseIndex(i+2)];
			vertices[j+1] = points[_nextBaseIndex(i+1)];
			j+= 3;
		}
		mesh.vertices = vertices;

		int[] triangles = new int[numberOfVertices];
		for(i=0; i<numberOfVertices; i++) {
			triangles[i] = i;
		}
		mesh.triangles = triangles;

		mesh.uv = _CalculateUV ();

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();

		MeshCollider collider = GetComponent<MeshCollider> ();
		collider.sharedMesh = mesh;
		collider.convex = true;
	}

	private int _nextBaseIndex(int index) {
		int i = (index + 1) % (baseSides + 1);
		return i == 0 ? 1 : i;
	}
}
