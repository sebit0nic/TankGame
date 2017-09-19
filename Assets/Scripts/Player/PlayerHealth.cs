using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int currentHealth = 5;
	public float invincibleTime = 0.5f;
	public GameObject actorExplosion;

	public bool invincibility = false;

	private GameManager gameManager;
	private float invincibilityTimer;
	private bool isInvincible = false;

	private void Awake() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		UpdateText ();
	}

	private void Update() {
		if (invincibilityTimer < Time.time) {
			isInvincible = false;
		}
	}

	public void DecreaseCurrentHealth(int value) {
		if (!isInvincible && !invincibility) {
			currentHealth -= value;
			UpdateText ();

			invincibilityTimer = Time.time + invincibleTime;
			isInvincible = true;
		}
	}

	private void UpdateText() {
		gameManager.NotifyPlayerHit (currentHealth);
		if (currentHealth == 0) {
			Instantiate (actorExplosion, transform.position, Quaternion.identity);
			gameManager.NotifyPlayerDestroyed ();
			gameObject.SetActive (false);
		}
	}
}
