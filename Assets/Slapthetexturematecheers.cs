using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slapthetexturematecheers : MonoBehaviour {

    Mesh mesh;
    
	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        List<Vector3> Vertices = new List<Vector3>(mesh.vertices);
        List<Vector2> UVs = new List<Vector2>();
        for (int i = 0; i < Vertices.Count; i++) {
            Debug.Log(Vertices[i].x * 1000 + ", " + Vertices[i].z * 1000);
            UVs.Add(new Vector2(Vertices[i].x * 100, Vertices[i].z * 100));
        }
        mesh.uv = UVs.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
