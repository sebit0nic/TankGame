using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShotPlayer : MonoBehaviour {

	public int damage = 10;
	public float hitDelay = 0.5f;

	private void OnTriggerEnter(Collider other) {
		Enemy hitEnemy = other.gameObject.GetComponent<Enemy> ();
		if (hitEnemy != null) {
			StartCoroutine (DamageEnemy (hitEnemy));
		}
	}

	private IEnumerator DamageEnemy(Enemy enemy) {
		yield return new WaitForSeconds (hitDelay);
		enemy.HitByProjectile (damage);
	}
}
