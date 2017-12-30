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
	private int floorMask, obscuranceMask;
	private float camRayLength = 200f;
	private bool usesAbility = false;

	private void Awake() {
		playerShootingScript = GetComponent<PlayerShootingScript> ();
		thisRigidbody = GetComponent<Rigidbody> ();

		floorMask = LayerMask.GetMask ("Floor");
		obscuranceMask = LayerMask.GetMask ("Enemy", "Environment");
	}

	private void Update() {
		if (Input.GetKey (KeyCode.Mouse0)) {
			playerShootingScript.Shoot ();
		}

		if (!usesAbility) {
			Turn ();
		}
	}

	private void FixedUpdate() {
		if (!usesAbility) {
			float vertical = Input.GetAxisRaw ("Vertical");
			float horizontal = Input.GetAxisRaw ("Horizontal");
			Vector3 moveDirection = new Vector3 (horizontal, 0, vertical);
			bool stopped = false;

			if (vertical == 0 && horizontal == 0) {
				stopped = true;
			}

			if (!stopped) {
				Quaternion newRotation = Quaternion.LookRotation (moveDirection);
				body.rotation = Quaternion.Slerp (body.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);
				thisRigidbody.MovePosition (thisRigidbody.position + body.forward * movementSpeed * Time.fixedDeltaTime);
			}
		}
	}

	private void Turn() {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		Ray obscuranceRay = new Ray (barrelEnd.transform.position, barrelEnd.transform.forward);
		RaycastHit floorHit;
		RaycastHit obscuranceHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			float playerToMouseDistance = Vector3.Distance (floorHit.point, transform.position);
			playerToMouse.y = 0;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			barrel.rotation = newRotation;

			if (Physics.Raycast (obscuranceRay, out obscuranceHit, playerToMouseDistance, obscuranceMask)) {
				playerShootingScript.UpdateTargetingLine (floorHit, obscuranceHit);
			} else {
				playerShootingScript.UpdateTargetingLine (floorHit, floorHit);
			}
		}
	}

	public void SetUsesAbility (bool newValue) {
		usesAbility = newValue;
	}
}
