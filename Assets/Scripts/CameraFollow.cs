using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing = 3f;
	public float mouseDamper = 0.5f;
	public Vector3 minPosition, maxPosition;

	private Vector3 offset;
	private int floorMask;
	private float camRayLength = 200f;
	private GameManager gameManager;

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

		Vector3 targetCamPos = target.position + offset + (playerToMouse * mouseDamper);
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);

		transform.position = new Vector3 (
			Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x),
			Mathf.Clamp(transform.position.y, minPosition.y, maxPosition.y),
			Mathf.Clamp(transform.position.z, minPosition.z, maxPosition.z));
	}

	public void OutroAnimationFinished() {
		gameManager.NotifyCameraOutroAnimationFinished ();
	}
}
