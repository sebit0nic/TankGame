using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int currentHealth = 3;
	public float invincibleTime = 1f;
	public Healthbar healthbar;
	public GameObject actorExplosion;
	public GameObject shield;

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
			shield.SetActive (false);
		}
	}

	public void DecreaseCurrentHealth(int value) {
		if (!isInvincible) {
			currentHealth -= value;
			UpdateText ();

			invincibilityTimer = Time.time + invincibleTime;
			isInvincible = true;
			shield.SetActive (true);
		}
	}

	private void UpdateText() {
		healthbar.UpdateHealthbar (currentHealth);
		if (currentHealth == 0) {
			Instantiate (actorExplosion, transform.position, Quaternion.identity);
			gameManager.NotifyPlayerDestroyed ();
			gameObject.SetActive (false);
		}
	}
}
