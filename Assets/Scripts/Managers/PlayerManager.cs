using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public Healthbar healthbar;

	private float currentHealth;
	private GameObject player;

	private void Awake() {
		healthbar.UpdateUI (currentHealth, 100);
		player = GameObject.Find ("Player").gameObject;
	}

	public void InitPosition (Vector3 startPosition) {
		player.transform.position = startPosition;
	}

	public void NotifyPlayerHealthUpdate(float health) {
		currentHealth = health;
		healthbar.UpdateUI (currentHealth, 100);
	}
}
