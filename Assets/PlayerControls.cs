using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

	Rigidbody rigidBody;
	Transform cameraTransform;
	
	float speed = 5f;
	float sensitivity = 5f;
	float jumpHeight = 10f;
	float groundDist = 0.5f;
	bool invertX = false;
	bool invertY = true;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		//transform = GetComponent<Transform>();
		cameraTransform = transform.GetChild(0).GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update () {

 		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
		float rotationY = cameraTransform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity;

		if (invertX) rotationX = transform.localEulerAngles.y - Input.GetAxis("Mouse X") * sensitivity;
		else rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;

		if (invertY) rotationY = cameraTransform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity;
		else rotationY = cameraTransform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity;

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, transform.localEulerAngles.z);
		cameraTransform.localEulerAngles = new Vector3(rotationY, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);

		rigidBody.velocity = new Vector3(Input.GetAxis("Horizontal") * speed * Mathf.Cos(rotationX * Mathf.Deg2Rad) + Input.GetAxis("Vertical") * speed * Mathf.Sin(rotationX * Mathf.Deg2Rad), rigidBody.velocity.y, Input.GetAxis("Vertical") * speed * Mathf.Cos(rotationX * Mathf.Deg2Rad) - Input.GetAxis("Horizontal") * speed * Mathf.Sin(rotationX * Mathf.Deg2Rad));
		Vector3 floorPosition = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);
		RaycastHit hit;
		bool onGround = Physics.Raycast(floorPosition, Vector3.down, out hit, groundDist);
		if (onGround && hit.transform.gameObject.tag == "bouncePad") {
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, bounceHeight, rigidBody.velocity.z);
		}
		else if (Input.GetKey(KeyCode.Space) && onGround && Mathf.Abs(rigidBody.velocity.y) < 5) {
			//Debug.Log("aaa");
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpHeight, rigidBody.velocity.z);
		}
		Debug.Log(rigidBody.velocity.y);
	}
}
