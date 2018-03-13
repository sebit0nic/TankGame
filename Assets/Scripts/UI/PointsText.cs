using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsText : MonoBehaviour {

	public float floatSpeed = 1, floatTime = 1;
	public Transform thisTransform;

	private TextMesh thisText;
	private Transform cameraTransform;

	private void Awake() {
		thisText = GetComponent<TextMesh> ();
		cameraTransform = Camera.main.transform;
	}

	private void Update() {
		thisTransform.Translate (0, Time.deltaTime * floatSpeed, 0);
		thisTransform.LookAt (cameraTransform);
		StartCoroutine (WaitForFloatTime());
	}

	public void SetText(string text) {
		thisText.text = text;
	}

	private IEnumerator WaitForFloatTime() {
		yield return new WaitForSeconds (floatTime);
		gameObject.SetActive (false);
	}
}
