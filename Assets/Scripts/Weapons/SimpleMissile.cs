﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMissile : MonoBehaviour {

	public bool playerFired = true;

	private Projectile thisProjectile;
	private Rigidbody thisRigidbody;
	private Collider thisCollider;

	private void Awake() {
		thisProjectile = GetComponent<Projectile> ();
		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<Collider> ();
	}

	public void Init(Vector3 position, Quaternion rotation, Vector3 force, ForceMode mode) {
		transform.position = position;
		transform.rotation = rotation;
		thisRigidbody.isKinematic = false;
		thisRigidbody.AddForce (force, mode);
		thisCollider.enabled = true;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Environment")) {
			thisProjectile.OnProjectileDestroy ();
		}

		if (playerFired) {
			if (other.gameObject.tag.Equals ("Enemy")) {
				thisProjectile.OnProjectileDestroy ();

				other.GetComponent<Enemy> ().HitByProjectile ();
			}
		} else {
			if (other.gameObject.tag.Equals ("Player")) {
				thisProjectile.OnProjectileDestroy ();

				PlayerHealth ph = other.GetComponent<PlayerHealth> ();
				ph.DecreaseCurrentHealth (1);
			}
		}
	}
}