using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int currentHealth = 3;
	public Text healthText;
	public IngameMenuControls imc;
	public GameObject actorExplosion;

	private void Awake() {
		UpdateText ();
	}

	public void DecreaseCurrentHealth(int value) {
		currentHealth -= value;
		UpdateText ();
	}

	private void UpdateText() {
		healthText.text = "Health: " + currentHealth;
		if (currentHealth == 0) {
			imc.ShowGameoverText ();
			Instantiate (actorExplosion, transform.position, Quaternion.identity);
			gameObject.SetActive (false);
		}
	}
}
