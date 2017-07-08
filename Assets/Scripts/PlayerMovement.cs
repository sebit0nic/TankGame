using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 1;
	public float rotationSpeed = 1;
	public float projectileSpeed = 500f;
	public Transform body;
	public Transform barrel;
	public Transform barrelEnd;
	public Rigidbody projectile;

	private Rigidbody thisRigidbody;
	private LineRenderer thisLineRenderer;
	private int floorMask;
	private float camRayLength = 100f;

	private void Awake() {
		thisRigidbody = GetComponent<Rigidbody> ();
		thisLineRenderer = GetComponent<LineRenderer> ();

		floorMask = LayerMask.GetMask ("Floor");
	}

	private void Update() {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Rigidbody newProjectile = Instantiate (projectile, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
			newProjectile.AddForce (newProjectile.transform.forward * projectileSpeed, ForceMode.Force);
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
			thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
			thisLineRenderer.SetPosition (1, new Vector3 (floorHit.point.x, barrelEnd.position.y, floorHit.point.z));
		}
	}
}
