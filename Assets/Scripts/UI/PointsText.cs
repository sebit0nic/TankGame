using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsText : MonoBehaviour {

	public float floatSpeed = 1, floatTime = 1;

	private TextMesh thisText;

	private void Awake() {
		thisText = GetComponent<TextMesh> ();
	}

	private void Update() {
		transform.Translate (0, Time.deltaTime * floatSpeed, 0);
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
