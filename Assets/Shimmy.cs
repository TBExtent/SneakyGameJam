using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shimmy : MonoBehaviour {

private Vector3 floatY;
public float height = 0.25f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		floatY = transform.position;
		floatY.y = ((Mathf.Sin(Time.time) + 1) * height);
		transform.position = floatY;
	}
}
