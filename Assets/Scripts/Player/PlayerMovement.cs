using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public enum PlayerBody {BODY_1, BODY_2, BODY_3, BODY_4, BODY_5}
	public PlayerBody currentPlayerBody;
	public GameObject[] playerBodies;

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

		switch (currentPlayerBody) {
		case PlayerBody.BODY_1:
			playerBodies [0].SetActive (true);
			break;
		case PlayerBody.BODY_2:
			playerBodies [1].SetActive (true);
			break;
		case PlayerBody.BODY_3:
			playerBodies [2].SetActive (true);
			break;
		case PlayerBody.BODY_4:
			playerBodies [3].SetActive (true);
			break;
		case PlayerBody.BODY_5:
			playerBodies [4].SetActive (true);
			break;
		}
	}

	private void Update() {
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
