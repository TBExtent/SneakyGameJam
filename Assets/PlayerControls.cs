using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

	Rigidbody rigidBody;
	Transform cameraTransform;
	//ParticleSystem particleSystem;
	ParticleSystem.EmissionModule emission;
	
	public float speed = 6f;
	public float sensitivity = 5f;
	public float jumpHeight = 7f;
	public float bounceHeight = 20f;
	float groundDist = 0.5f;
	public bool invertX = false;
	public  bool invertY = true;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		//transform = GetComponent<Transform>();
		cameraTransform = transform.GetChild(0).GetComponent<Transform>();
		//particleSystem = GetComponent<ParticleSystem>();
		//emission = particleSystem.emission;
		//emission.enabled = true;

		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update () {

 		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
		float rotationY = cameraTransform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity;

		if (invertX) rotationX = transform.localEulerAngles.y - Input.GetAxis("Mouse X") * sensitivity;
		else rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;

		if (invertY) rotationY = cameraTransform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity;
		else rotationY = cameraTransform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity;
		if (rotationY > 90 && rotationY < 180) rotationY = 90;
		else if (rotationY < 270 && rotationY >= 180) rotationY = 270;

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, transform.localEulerAngles.z);
		cameraTransform.localEulerAngles = new Vector3(rotationY, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);

		//if (!Physics.Raycast(transform.position, new Vector3(Input.GetAxis("Horizontal") * Mathf.Cos(rotationX * Mathf.Deg2Rad) + Input.GetAxis("Vertical") * Mathf.Sin(rotationX * Mathf.Deg2Rad), rigidBody.velocity.y, Input.GetAxis("Vertical") * speed * Mathf.Cos(rotationX * Mathf.Deg2Rad) - Input.GetAxis("Horizontal") * speed * Mathf.Sin(rotationX * Mathf.Deg2Rad)), 1f)) {
		rigidBody.velocity = new Vector3(Input.GetAxis("Horizontal") * speed * Mathf.Cos(rotationX * Mathf.Deg2Rad) + Input.GetAxis("Vertical") * speed * Mathf.Sin(rotationX * Mathf.Deg2Rad), rigidBody.velocity.y, Input.GetAxis("Vertical") * speed * Mathf.Cos(rotationX * Mathf.Deg2Rad) - Input.GetAxis("Horizontal") * speed * Mathf.Sin(rotationX * Mathf.Deg2Rad));
		//}
		Vector3 floorPosition = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);
		RaycastHit hit;
		bool onGround = Physics.Raycast(floorPosition, Vector3.down, out hit, groundDist);
		if (onGround && hit.transform.gameObject.tag == "bouncePad") {
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, bounceHeight, rigidBody.velocity.z);
		}
		else if (Input.GetKey(KeyCode.Space) && onGround && Mathf.Abs(rigidBody.velocity.y) < 5) {
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpHeight, rigidBody.velocity.z);
		}

		//if (Input.GetKey(KeyCode.Mouse0)) GunParticles();
	}

	void GunParticles () {
		//emission.SetBursts(new ParticleSystem.Burst[]{new ParticleSystem.Burst(0.2f, 20, 30, 1, 1)});
		//particleSystem.Play();
	}
}
