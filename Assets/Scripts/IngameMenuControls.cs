using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenuControls : MonoBehaviour {

	public GameObject gameoverText;

	private void Update() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			SceneManager.LoadScene (0, LoadSceneMode.Single);
		}
	}

	public void ShowGameoverText() {
		gameoverText.SetActive (true);
	}
}
