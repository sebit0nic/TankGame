using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float currentHealth = 5;
	public float invincibleTime = 0.5f;
	public GameObject actorExplosion;
	public float slowDownDuration;
	public bool invincibility = false;
	public AnimationCurve healthRegeneration;

	private GameManager gameManager;
	private float invincibilityTimer;
	private bool isInvincible = false;
	private float slowDownValue = 0;

	private void Awake() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		UpdateText (false);
	}

	private void Update() {
		if (invincibilityTimer < Time.time) {
			isInvincible = false;
		}

		currentHealth += (healthRegeneration.Evaluate(gameManager.GetCurrentHeatFactor() / 100) * 10) * Time.deltaTime;
		UpdateText (false);
		if (currentHealth > 100) {
			currentHealth = 100;
		}
	}

	public void DecreaseCurrentHealth(int value) {
		if (!isInvincible && !invincibility) {
			currentHealth -= value;
			UpdateText (true);

			invincibilityTimer = Time.time + invincibleTime;
			isInvincible = true;

			StartCoroutine (OnSlowDownEffect ());
		}
	}

	private void UpdateText(bool damaged) {
		gameManager.NotifyPlayerHealthUpdate (currentHealth, damaged);
		if (currentHealth == 0) {
			Instantiate (actorExplosion, transform.position, Quaternion.identity);
			gameManager.NotifyPlayerDestroyed ();
			gameObject.SetActive (false);
		}
	}

	private IEnumerator OnSlowDownEffect() {
		Time.timeScale = slowDownValue;
		yield return new WaitForSecondsRealtime (slowDownDuration);
		Time.timeScale = 1;
	}

	public void SetInvincible (bool newValue) {
		isInvincible = newValue;
	}
}
