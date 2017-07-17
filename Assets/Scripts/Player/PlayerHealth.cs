using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int currentHealth = 3;
	public float invincibleTime = 1f;
	public Text healthText;
	public IngameMenuControls imc;
	public Healthbar healthbar;
	public GameObject actorExplosion;
	public GameObject shield;

	private float invincibilityTimer;
	private bool isInvincible = false;

	private void Awake() {
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
			imc.ShowGameoverText ();
			Instantiate (actorExplosion, transform.position, Quaternion.identity);
			gameObject.SetActive (false);
		}
	}
}
