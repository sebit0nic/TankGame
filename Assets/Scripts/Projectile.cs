using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject explosion;
	public GameObject actorExplosion;

	public bool playerFired = true;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Environment")) {
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}

		if (playerFired) {
			if (other.gameObject.tag.Equals ("Enemy")) {
				Instantiate (explosion, transform.position, Quaternion.identity);

				other.GetComponent<Enemy> ().HitByProjectile ();
				Destroy (gameObject);
			}
		} else {
			if (other.gameObject.tag.Equals ("Player")) {
				Instantiate (explosion, transform.position, Quaternion.identity);

				PlayerHealth ph = other.GetComponent<PlayerHealth> ();
				ph.DecreaseCurrentHealth (1);
				Destroy (gameObject);
			}
		}
	}
}
