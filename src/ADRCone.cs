using UnityEngine;
using UnityEditor;
using System.Collections;

[RequireComponent (typeof (MeshCollider))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class ADRCone : MonoBehaviour {

	private float _radius = 1.0f;
	private float _height = 2.0f;
	private int _baseSides = 20;
	
	private static Material _defaultDiffuse = null;
	
	public static void Create() {
		Create (Vector3.zero, Quaternion.identity);
	}
	
	public static void Create(Vector3 position, Quaternion rotation) {
		GameObject gameObject = new GameObject("Cone");
		ADRCone cone = gameObject.AddComponent<ADRCone>();
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh();
		cone.Rebuild();
		
		if (_defaultDiffuse == null) {
			GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
			_defaultDiffuse = primitive.GetComponent<MeshRenderer>().sharedMaterial;
			DestroyImmediate(primitive);
		}
		cone.GetComponent<Renderer> ().sharedMaterial = _defaultDiffuse;
	}
	
	private Vector3[] _CalculatePoints() {
		float halfHeight = _height / 2;
		Vector3[] points = new Vector3[_baseSides + 1];
		points [0] = new Vector3 (0, halfHeight, 0);
		
		int i;
		float singleAngle = 360f / _baseSides * Mathf.Deg2Rad;
		for (i=1; i<points.Length; i++) {
			float angle = singleAngle * (i-1);
			points[i] = new Vector3(Mathf.Cos (angle) * _radius, -halfHeight, Mathf.Sin (angle) * _radius);
		}
		
		return points;
	}

	private Vector2[] _CalculateUV() {
		Vector2[] uv = new Vector2[2 * _baseSides + 1];
		float singleAngle = 360f / _baseSides * Mathf.Deg2Rad;
		Vector2 center = new Vector2 (0.25f, 0.5f);
		uv [0] = center;

		int i, j;
		float angleSum = Mathf.PI + (singleAngle / 2);
		for(i=0; i < _baseSides; i++) {
			uv[i+1] = center + (new Vector2(Mathf.Cos (angleSum) * 0.25f, Mathf.Sin (angleSum) * 0.25f));
			angleSum -= singleAngle;
		}

		angleSum = Mathf.PI + (singleAngle / 2);
		Vector2 translation = new Vector2 (0.5f, 0);
		for (j = 0; j < _baseSides; j++) {
			uv[i + j + 1] = uv[j+1] + translation;
		}

		return uv;
	}

	public void Rebuild() {
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			Debug.LogError("[ADRCone] - MeshFilter not found!");
			return;
		}
		
		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.Clear();
		
		Vector3[] points = _CalculatePoints ();

		int numberOfVertices = 2 * _baseSides + 1;
		Vector3[] vertices = new Vector3[numberOfVertices];
		points.CopyTo (vertices, 0);

		int i, j;
		for (i = 0; i<_baseSides; i++) {
			vertices[_baseSides + 1 + i] = points[i + 1];
		}
		mesh.vertices = vertices;

		int halfNumberOfTriangles = _baseSides - 1;
		int[] triangles = new int[halfNumberOfTriangles * 6];
		j = 0;
		for(i=0; i<halfNumberOfTriangles + 1; i++) {
			triangles[j] = 0;
			triangles[j + 1] = i + 2 == _baseSides + 1 ? 1 : i + 2;
			triangles[j + 2] = i + 1;
			j += 3;
		}
		for (i=0; i<halfNumberOfTriangles - 1; i++) {
			triangles[j] = _baseSides + 1;
			triangles[j+1] = _baseSides + 2 + i;
			triangles[j+2] = _baseSides + 3 + i;
			j += 3;
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
		int i = (index + 1) % (_baseSides + 1);
		return i == 0 ? 1 : i;
	}
}

