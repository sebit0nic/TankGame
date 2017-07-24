using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public Healthbar healthbar;

	private int currentHealth;

	private void Awake() {
		healthbar.UpdateUI (currentHealth);
	}

	public void NotifyPlayerHit(int health) {
		currentHealth = health;
		healthbar.UpdateUI (currentHealth);
	}
}
