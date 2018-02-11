using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShotPlayer : MonoBehaviour {

	public int damage = 10;

	private void OnTriggerEnter(Collider other) {
		Enemy hitEnemy = other.gameObject.GetComponent<Enemy> ();
		if (hitEnemy != null) {
			hitEnemy.HitByProjectile (damage);
		}
	}
}
