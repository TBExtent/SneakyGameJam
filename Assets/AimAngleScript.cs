using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAngleScript : MonoBehaviour {

	public float AimAngle = 0;
	public GameObject PlayerCamera;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		AdjustAimAngle();
	}

	void AdjustAimAngle()
{
		if(PlayerCamera.transform.rotation.eulerAngles.x <= 90f)
		{
				//Looking down
				AimAngle = PlayerCamera.transform.rotation.eulerAngles.x * -1 + 5;
		}
		else
		{
				//lookingUp
				AimAngle = 360 - PlayerCamera.transform.rotation.eulerAngles.x +5;
		}
	//	anim.SetFloat("AimAngle", aimAngle);
}
}
