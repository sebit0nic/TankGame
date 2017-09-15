using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour, Level {

	public PlayerHealth playerHealth;
	//TODO: Add a "Treelist" to watch how many trees have been destroyed for secondary task 2
	public ScoreManager scoreManager;

	private GameManager gameManager;
	private bool ninerCombo = false;

	private void Start() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	private void FixedUpdate() {
		if (scoreManager.currentCombo >= 9) {
			ninerCombo = true;
		}
	}

	public void OnLevelStart () {
	}

	public void OnLevelEnd () {
		if (playerHealth.currentHealth == 3) {
			gameManager.NotifySecondaryTaskOneSuccessful ();
		}

		//TODO: Implement secondary task 2

		if (ninerCombo) {
			gameManager.NotifySecondaryTaskThreeSuccessful ();
		}
	}
}
