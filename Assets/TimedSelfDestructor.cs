using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSelfDestructor : MonoBehaviour {
    public float time = 1f;
	// Use this for initialization
	void Start () {
        Invoke("destruct", time);
	}
	
	// Update is called once per frame
	void destruct()
    {
        Destroy(gameObject);
    }
}
