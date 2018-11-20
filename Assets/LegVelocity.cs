using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegVelocity : MonoBehaviour {

	Vector3 PreviousFramePosition = Vector3.zero; // Or whatever your initial position is
  public float Speed = 0f;
	public Animator leg1;
	public Animator leg2;
  void Update () {
      // All the update stuff
      // ...
      float movementPerFrame = Vector3.Distance (PreviousFramePosition, transform.position) ;
      Speed = movementPerFrame / Time.deltaTime;
      PreviousFramePosition = transform.position;
			leg1.SetFloat("Speed", Speed);
			leg2.SetFloat("Speed", Speed);
  }
}
