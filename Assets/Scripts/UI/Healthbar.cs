using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

	private Image healthBar;

	private void Awake() {
		healthBar = GetComponent<Image> ();
	}

	public void UpdateUI (float currentHealth, float maxHealth) {
		healthBar.fillAmount = currentHealth / maxHealth;
	}
}
