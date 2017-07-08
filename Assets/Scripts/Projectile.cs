using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject explosion;
	public GameObject enemyExplosion;

	public bool playerFired = true;

	private IngameMenuControls imc;

	private void Awake() {
		imc = GameObject.Find ("Canvas").GetComponent<IngameMenuControls> ();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Environment")) {
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}

		if (playerFired) {
			if (other.gameObject.tag.Equals ("Enemy")) {
				Instantiate (explosion, transform.position, Quaternion.identity);
				Instantiate (enemyExplosion, other.transform.position, Quaternion.identity);
				Destroy (other.gameObject);
				Destroy (gameObject);
			}
		} else {
			if (other.gameObject.tag.Equals ("Player")) {
				Instantiate (explosion, transform.position, Quaternion.identity);
				Instantiate (enemyExplosion, other.transform.position, Quaternion.identity);
				other.gameObject.SetActive (false);
				imc.ShowGameoverText ();
				Destroy (gameObject);
			}
		}
	}
}
