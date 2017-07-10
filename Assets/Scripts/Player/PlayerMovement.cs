using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 1;
	public float rotationSpeed = 1;
	public Transform body;
	public Transform barrel;
	public Transform barrelEnd;

	private PlayerShootingScript playerShootingScript;
	private Rigidbody thisRigidbody;
	private int floorMask;
	private float camRayLength = 100f;

	private void Awake() {
		playerShootingScript = GetComponent<PlayerShootingScript> ();
		thisRigidbody = GetComponent<Rigidbody> ();

		floorMask = LayerMask.GetMask ("Floor");
	}

	private void Update() {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			playerShootingScript.Shoot ();
		}
	}

	private void FixedUpdate() {
		Move ();
		Turn ();
	}

	private void Move() {
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		Vector3 moveDirection = new Vector3 (horizontal, 0, vertical);

		thisRigidbody.MovePosition (thisRigidbody.position + moveDirection * movementSpeed * Time.deltaTime);
		if (moveDirection != Vector3.zero) {
			Quaternion newRotation = Quaternion.LookRotation (moveDirection);
			body.rotation = Quaternion.Slerp (body.rotation, newRotation, Time.deltaTime * rotationSpeed);
		}
	}

	private void Turn() {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			barrel.rotation = newRotation;

			playerShootingScript.UpdateTargetingLine (floorHit);
		}
	}
}
