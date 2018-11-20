using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritColour : MonoBehaviour {
	[Header("Element 1 corresponds to RED, 2 to Blue")]
	public Texture[] CharacterTextures;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetColour (int colour) {
		Transform coat = transform.Find("Torso/Coat");
		Renderer renderer = coat.GetComponent<Renderer>();
		renderer.material.mainTexture = CharacterTextures[colour];
	}
}
