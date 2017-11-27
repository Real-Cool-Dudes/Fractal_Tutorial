using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {
	public Mesh mesh;
	public Material material;

	public int maxDepth;
	private int depth;

	public float childScale;

	private Material[] materials;

	private void Start() {
		gameObject.AddComponent<MeshFilter> ().mesh = mesh;
		gameObject.AddComponent<MeshRenderer> ().material = material;
		GetComponent<MeshRenderer> ().material.color =
			Color.Lerp (Color.white, Color.yellow, (float)depth / maxDepth);
		if (depth < maxDepth) {
			StartCoroutine (CreateChildren ());
		}
	}

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};
	
	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f, 0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f)
	};

	private IEnumerator CreateChildren() {
		for(int i = 0; i < childDirections.Length; i++) {
			yield return new WaitForSeconds (Random.Range(0.1f,0.5f));
			new GameObject ("FractalChild").AddComponent<Fractal> ().Initialize(this,i);

		}

	}

	private void Initialize(Fractal parent, int childIndex) {
		mesh = parent.mesh;
		material = parent.material;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		transform.parent = parent.transform;
		transform.localRotation = childOrientations[childIndex];
		childScale = parent.childScale;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
	}
}
