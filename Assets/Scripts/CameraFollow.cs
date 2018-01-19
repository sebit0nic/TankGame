using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing = 3f;
	public float mouseDamper = 0.5f;
	public Vector3 minCameraBoundaries, maxCameraBoundaries;

	private Vector3 offset;
	private int floorMask;
	private float camRayLength = 200f;
	private GameManager gameManager;
	private bool followMouse = true;

	private void Awake() {
		offset = transform.position - target.position;
		floorMask = LayerMask.GetMask ("Floor");
	}

	private void Start() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	private void FixedUpdate() {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		Vector3 playerToMouse = Vector3.zero;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			playerToMouse = floorHit.point - target.position;
			playerToMouse.y = 0;
		}

		Vector3 targetCamPos;
		if (followMouse) {
			targetCamPos = target.position + offset + (playerToMouse * mouseDamper);
		} else {
			targetCamPos = target.position + offset;
		}

		Vector3 tempPos = targetCamPos;
		tempPos.x = Mathf.Clamp (tempPos.x, minCameraBoundaries.x, maxCameraBoundaries.x);
		tempPos.z = Mathf.Clamp (tempPos.z, minCameraBoundaries.z, maxCameraBoundaries.z);
		targetCamPos = tempPos;

		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}

	public void OutroAnimationFinished() {
		gameManager.NotifyCameraOutroAnimationFinished ();
	}

	public void SetFollowMouse(bool newValue) {
		followMouse = newValue;
	}
}
