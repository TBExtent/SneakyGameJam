using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

	Rigidbody rigidBody;
	Transform transform;
	Transform cameraTransform;
	float speed = 5f;
	float sensitivity = 3f;
	float minimumY = -60f;
	float maximumY = 60f;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		transform = GetComponent<Transform>();
		cameraTransform = transform.GetChild(0).GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		rigidBody.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, rigidBody.velocity.y, Input.GetAxis("Vertical") * speed);

		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
		float rotationY = cameraTransform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity;
		//rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, transform.localEulerAngles.z);
		//cameraTransform.localEulerAngles = new Vector3(rotationY, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);
	}
}
