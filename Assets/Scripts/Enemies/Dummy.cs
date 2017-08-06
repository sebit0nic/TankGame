using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, Enemy {

	private GameManager gameManager;

	private void Awake() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
	}

	public void HitByProjectile() {
		//TODO: play destroyed animation/particle effect
		gameManager.NotifyEnemyDestroyed (0);
		gameObject.SetActive (false);
	}
}
